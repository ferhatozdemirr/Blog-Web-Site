using Blog.Data.Context;
using Blog.Data.Repositories.Abstractions;
using Blog.Data.Repositories.Concretes;
using Blog.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Data.Extensions
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection LoadDataLayerExtensions(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitofWork, UnitOfWork>();


            return services;
        }
    }
}





    //Bağımlılık tanımlarını yaparken 3 adet farklı yaşam döngüsü bulunmaktadır.
    //}
    //Transient(AddTransient)
    //Uygulama içerisinde bağımlılık olarak oluşturduğumuz ve kullandığımız nesnenin her kullanım ve çağrıda tekrardan oluşturulmasını sağlar.

//Singleton (AddSingleton)
//Uygulama içerisinde bağımlılık oluşturduğumuz ve kullandığımız nesnenin tek bir sefer oluşturulmasını ve aynı nesnenin uygulama içinde kullanılmasını sağlar.

//Scoped (AddScoped)
//Uygulama içerisindeki bağımlılık oluşturduğumu nesnenin request sonlanana kadar aynı nesneyi kullanmasını farklı bir çağrı için gelindiğinde yeni bir nesne yaratılmasını sağlar.
