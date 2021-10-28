using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IHouseRepository
    {
        Task<House> CreateHouse(House house);
        IEnumerable<House> GetAllHouses();
        House GetHouseById(string user);
        IEnumerable<House> GetHousesByPriceRange(int lowPrice, int HighPrice);
    }
}
