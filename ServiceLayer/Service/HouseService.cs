using DAL.Interface;
using DAL.Repository;
using Domain;
using Microsoft.Extensions.Logging;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class HouseService : IHouseService
    {
        private readonly IHouseRepository _HouseRepository;
        private readonly ILogger<HouseService> _Logger;

        public HouseService(ILogger<HouseService> logger, IHouseRepository houseRepository)
        {
            _Logger = logger;
            _HouseRepository = houseRepository;
        }

        public async Task<House> CreateHouse(House house)
        {
            house.id = Guid.NewGuid().ToString();
            return await _HouseRepository.CreateHouse(house);
        }

        public House GetHouseById(string id)
        {
            return _HouseRepository.GetHouseById(id);
        }

        public IEnumerable<House> GetHouses()
        {
            return _HouseRepository.GetAllHouses();
        }

        public IEnumerable<House> GetHousesBetweenPrice(int low, int high)
        {
            return _HouseRepository.GetHousesByPriceRange(low, high);
        }
    }
}
