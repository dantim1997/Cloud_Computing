using DAL.EF;
using DAL.Interface;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class HouseRepository : IHouseRepository
    {
        private HouseContext _HouseContext;

        public HouseRepository(HouseContext userContext)
        {
            _HouseContext = userContext;
        }

        // create the house
        public async Task<House> CreateHouse(House house)
        {
            _HouseContext.Add<House>(house);
            await _HouseContext.SaveChangesAsync();

            return house;
        }

        // get the house by id
        public House GetHouseById(string user)
        {
            return _HouseContext.Houses.Find(user);
        }

        // get the houses by price range
        public IEnumerable<House> GetHousesByPriceRange(int lowPrice, int HighPrice)
        {
            return _HouseContext.Houses.Where(a => a.Price > lowPrice && a.Price < HighPrice).ToList();
        }

        // get all the houses from the db
        public IEnumerable<House> GetAllHouses()
        {
            var users = _HouseContext.Houses.ToList();
            return users;
        }
    }
}
