using Infrastracture;
using Application;
namespace BookCatalog.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();

            builder.Services.AddResponseCaching(); //using Response cache service
            builder.Services.AddOutputCache();// using output cache service
            builder.Services.AddMemoryCache(); // using In-Memory cache

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseResponseCaching(); // add respone caching middleware
            app.UseOutputCache();// add output caching middleware
            app.MapControllers();

            app.Run();
        }
    }
}
