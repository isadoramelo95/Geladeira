using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Context;
using Repository.Interfaces;
using Repository.RepositoryClasses;
using Service.Interface;
using Service.ServicesClasses;

namespace Service.DIP
{
    public class Initializer
    {
        public static void Configure(IServiceCollection services, string connection)
        {
            services.AddDbContext<GeladeiraContext>(options => options.UseSqlServer(connection));

            services.AddScoped(typeof(IGeladeiraRepository<Item>), typeof(GeladeiraRepository));
            services.AddScoped(typeof(IGeladeiraService<Item>), typeof(GeladeiraService));
        }
    }
}
