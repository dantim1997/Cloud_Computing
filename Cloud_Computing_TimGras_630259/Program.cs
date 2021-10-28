using DAL.BlobService;
using DAL.EF;
using DAL.Interface;
using DAL.Repository;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLayer.Interface;
using ServiceLayer.Service;
using System.Threading.Tasks;

namespace Cloud_Computing_TimGras_630259
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    // services
                    services.AddTransient<IUserService, UserService>();
                    services.AddTransient<IMortgageService, MortgageService>();
                    services.AddTransient<IHouseService, HouseService>();

                    // repositories
                    services.AddTransient<IUserRepository, UserRepository>();
                    services.AddTransient<IHouseRepository, HouseRepository>();

                    //EFContexts
                    services.AddDbContext<UserContext>();
                    services.AddDbContext<MortgageContext>();
                    services.AddDbContext<HouseContext>();


                    services.AddSingleton<IBlobStorage, BlobStorage>();
                })
                .ConfigureFunctionsWorkerDefaults()
                .Build();

            host.Run();
        }
    }
}