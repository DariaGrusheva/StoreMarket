
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using StoreMarket.Abstraction;
using StoreMarket.Contexts;
using StoreMarket.Mappers;
using StoreMarket.Serveces;

namespace StoreMarket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //добавляю для строки подключения
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var cfg = config.Build();
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBilder =>
            {
                containerBilder.Register(c => new StoreContext(cfg.GetConnectionString("db"))).InstancePerDependency();
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(c => c.RegisterType<ProductServices>()
                                                                .As<IProductServices>());
            
            builder.Services.AddMemoryCache(m=> m.TrackStatistics = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}