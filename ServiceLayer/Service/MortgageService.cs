using DAL.Interface;
using DAL.Repository;
using Domain;
using Helpers;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class MortgageService : IMortgageService
    {
        private readonly IUserService _UserService;
        private readonly IBlobStorage _BlobStorage;
        private readonly ILogger<MortgageService> _Logger;
        private readonly string _ServiceBusConnectString = Environment.GetEnvironmentVariable("AzureConnectionString");
        private readonly string _CreateQueueName = Environment.GetEnvironmentVariable("ServiceBusNameCreate");
        private readonly string _DeleteQueueName = Environment.GetEnvironmentVariable("ServiceBusNameDelete");

        public MortgageService(ILogger<MortgageService> logger, IUserService userService, IBlobStorage blobStorage)
        {
            _Logger = logger;
            _UserService = userService;
            _BlobStorage = blobStorage;
        }

        // create the mortgage pdf with the calculation
        public async Task CreateMortgage(string queueString)
        {
            var userids = System.Text.Json.JsonSerializer.Deserialize<List<string>>(queueString);

            foreach (var userid in userids)
            {
                var user = await _UserService.GetUserById(userid);
                //calculate mortgage(what they can borrow)
                var canLoan = user.Income * 3.52;
                var mortgage = new Mortgage()
                {
                    Email = user.Email,
                    id = Guid.NewGuid().ToString(),
                    MortgageAmount = canLoan,
                    ZipCode = user.ZipCode,
                    Name = user.Name
                };
                //Create file and save in fide the data above
                var pdf = PDFCreator.CreatePDF(mortgage);

                var fileId = await _BlobStorage.CreateFile(Convert.ToBase64String(pdf), Guid.NewGuid()+".pdf");
                user.FileId = fileId;
                await _UserService.EditUser(user);
                _Logger.LogInformation($"Mortgage for buyer: {user.Name} has been calculated and saved: {DateTime.Now}");
            }
        }

        // create the queue for the service bus to create the mortgages
        public async Task CreateMortgageQueue()
        {
            var users = await _UserService.GetAllUsers();

            if (!string.IsNullOrEmpty(_CreateQueueName))
            {
                IQueueClient client = new QueueClient(_ServiceBusConnectString, _CreateQueueName);

                //will take batch and give that to the service bus(improve speed)
                var batchListOfUsers = Helpers.SplitHelper.Split(users.ToList());

                //send user mortgage info to queue to be calculated(what they can borrow)
                foreach (var batchusers in batchListOfUsers)
                {
                    var messageBody = JsonConvert.SerializeObject(batchusers.Select(a => a.id));
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    await client.SendAsync(message);
                }
                _Logger.LogInformation($"{DateTime.Now} Mortgage has been calculated for {users.ToList().Count} users.");
            }
        }

        // send all the mortgages
        public async Task GetAllMortgages()
        {
            var users = await _UserService.GetAllUsers();
            //send all mortgages
            foreach (var user in users)
            {
                var linkString = await _BlobStorage.GetBlobFromServer(user.FileId);
                var email = new EmailAddress(user.Email);
                SendEmail.SendMail(email, "Mortgage", "", $"<a href='{linkString}'>See your mortgage.</a> available till 23:00");
            }

            _Logger.LogInformation($"Send all mails to the Users: {DateTime.Now}");
        }

        // Deletes the mortgage pdfs from the blob server
        public async Task DeleteMortgage(string queueString)
        {
            var fileIds = System.Text.Json.JsonSerializer.Deserialize<List<string>>(queueString);

            foreach (var fileId in fileIds)
            {
                await _BlobStorage.DeleteBlobFromServer(fileId);
            }
            _Logger.LogInformation($"{DateTime.Now}: Zet of Mortgage PDFs has been deleted.");
        }

        //reate the queue for the service bus to delete the mortgages
        public async Task DeleteMortgageQueue()
        {
            var users = await _UserService.GetAllUsers();

            if (!string.IsNullOrEmpty(_DeleteQueueName))
            {
                IQueueClient client = new QueueClient(_ServiceBusConnectString, _DeleteQueueName);

                var usersWithMortgage = users.ToList().Where(a => !string.IsNullOrEmpty(a.FileId)).ToList();
                //will take batch and give that to the service bus(improve speed)
                var batchListOfUsers = Helpers.SplitHelper.Split(usersWithMortgage);

                //send user mortgage info to queue to be calculated(what they can borrow)
                foreach (var batchusers in batchListOfUsers)
                {
                    var messageBody = JsonConvert.SerializeObject(batchusers.Select(a => a.FileId));
                    batchusers.ForEach(a => a.FileId = "");
                    await _UserService.EditUsers(batchusers);
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    await client.SendAsync(message);
                }
                _Logger.LogInformation($"{DateTime.Now} Mortgage has been queued for delete.");
            }
        }
    }
}
