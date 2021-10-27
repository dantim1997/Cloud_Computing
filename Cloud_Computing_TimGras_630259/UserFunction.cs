using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Cloud_Computing_TimGras_630259
{
    public class UserFunction
    {
        private readonly IUserService _UserService;
        private readonly ILogger<UserFunction> _Logger;
        public UserFunction(ILogger<UserFunction> logger, IUserService userService)
        {
            _Logger = logger;
            _UserService = userService;
        }

        // Creates a buyer To calculate the mortgage later
        [Function("CreateBuyer")]
        public async Task<HttpResponseData> CreateBuyer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req, FunctionContext executionContext,
            ILogger log)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var user = JsonConvert.DeserializeObject<User>(content);

                await _UserService.CreateBuyer(user);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                return response;
            }
            catch (Exception e)
            {
                _Logger.LogError("{Error}", e.Message);
                throw;
            }
        }
    }
}
