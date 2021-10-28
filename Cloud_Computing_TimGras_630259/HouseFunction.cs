using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceLayer.Interface;

namespace Cloud_Computing_TimGras_630259
{
    public class HouseFunction
    {
        private readonly IHouseService _HouseService;
        private readonly ILogger<HouseFunction> _Logger;
        public HouseFunction(ILogger<HouseFunction> logger, IHouseService houseService)
        {
            _Logger = logger;
            _HouseService = houseService;
        }

        [Function("CreateHouse")]
        public async Task<HttpResponseData> CreateHouse([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var house = JsonConvert.DeserializeObject<House>(content);

                var houses = await _HouseService.CreateHouse(house);
                var json = JsonConvert.SerializeObject(houses);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(json);
                return response;
            }
            catch (Exception e)
            {
                _Logger.LogError("{Error}", e.Message);
                throw;
            }
        }

        [Function("GetHousesBetweenPrice")]
        public async Task<HttpResponseData> GetHousesBetweenPrice([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext, int lowPrice, int highPrice)
        {
            try
            {
                var houses = _HouseService.GetHousesBetweenPrice(lowPrice, highPrice);
                var json = JsonConvert.SerializeObject(houses);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(json);
                return response;
            }
            catch (Exception e)
            {
                _Logger.LogError("{Error}", e.Message);
                throw;
            }
        }

        [Function("GetAllHouses")]
        public async Task<HttpResponseData> GetAllHouses([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                var houses = _HouseService.GetHouses();
                var json = JsonConvert.SerializeObject(houses);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(json);
                return response;
            }
            catch (Exception e)
            {
                _Logger.LogError("{Error}", e.Message);
                throw;
            }
        }

        [Function("GetHouseId")]
        public async Task<HttpResponseData> GetHouseId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext executionContext, string id)
        {
            try
            {
                var houses = _HouseService.GetHouseById(id);
                var json = JsonConvert.SerializeObject(houses);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(json);
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
