using DAL.Interface;
using Domain;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly IMortgageRepository _MortgageRepository;
        private readonly IUserService _UserService;
        private readonly ILogger<MortgageService> _Logger;

        public MortgageService(ILogger<MortgageService> logger, IMortgageRepository mortgageRepository, IUserService userService)
        {
            _MortgageRepository = mortgageRepository;
            _Logger = logger;
            _UserService = userService;
        }

        public async Task CreateMortgage(string queueString)
        {
            var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(queueString);

            foreach (var user in users)
            {
                //calculate mortgage(what they can borrow)
                //calculation is fake need to find a real one
                var canLoan = (user.Income * 0.52) * 10;
                var mortgage = new Mortgage()
                {
                    Email = user.Email,
                    id = Guid.NewGuid(),
                    MortgageAmount = canLoan,
                };
                await _MortgageRepository.CreateMortgage(mortgage);
                _Logger.LogInformation($"Mortgage for buyer: {user.Name} has been calculated and saved: {DateTime.Now}");
            }
        }

        public async Task CreateMortgageQueue()
        {
            var users = await _UserService.GetAllUsers();

            string ServiceBusConnectString = Environment.GetEnvironmentVariable("AzureConnectionString");
            string QueueName = Environment.GetEnvironmentVariable("ServiceBusName");
            if (!string.IsNullOrEmpty(QueueName))
            {
                IQueueClient client = new QueueClient(ServiceBusConnectString, QueueName);

                //will take batch and give that to the service bus(improve speed)
                var batchListOfUsers = Helpers.SplitHelper.Split(users.ToList());

                //send user mortgage info to queue to be calculated(what they can borrow)
                foreach (var batchusers in batchListOfUsers)
                {
                    var messageBody = JsonConvert.SerializeObject(batchusers);
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    await client.SendAsync(message);
                }
                _Logger.LogInformation($"{DateTime.Now} Mortgage has been calculated for {users.ToList().Count} users.");
            }
        }

        public async Task GetAllMortgages()
        {
            var mortgages = await _MortgageRepository.GetAllMortgages();
            //send all mortgages
            foreach (var mortgage in mortgages)
            {
                string hostLink = "http://localhost:7071/api/";
                var link = $"{hostLink}ViewMortgage?Mortgage={mortgage.id}";
                SendMail(mortgage.Email, link);
            }

            _Logger.LogInformation($"Send all mails to the Users: {DateTime.Now}");
        }


        public static void SendMail(string email, string body)
        {
            //var smtpClient = new SmtpClient("smtp.gmail.com")
            //{
            //    Port = 587,
            //    Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("EmailUsername"), Environment.GetEnvironmentVariable("EmailPassword")),
            //    EnableSsl = true,
            //};

            //smtpClient.Send("we@test.nl", email, "See your mortgage", body);
        }
    }
}
