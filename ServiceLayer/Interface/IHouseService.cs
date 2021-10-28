using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IHouseService
    {
        Task<House> CreateHouse(House house);
        House GetHouseById(string id);
        IEnumerable<House> GetHouses();
        IEnumerable<House> GetHousesBetweenPrice(int low, int high);
    }
}
