using Infrastracture;
using Application;
using Microsoft.Extensions.DependencyInjection;
using BookCatalog.Core.Api.CustomMiddleWares;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Timers;
using Microsoft.OpenApi.Models;
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

            // This configuration for checking token with Auhtorized or don't in Swagger we did that in Postman in last commit
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication wiht JWT Token",
                    Type = SecuritySchemeType.Http
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {

                    new OpenApiSecurityScheme()
                    {

                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        },
                    },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddStackExchangeRedisCache(setupAction =>
            {
                setupAction.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
            });

            //***********************AddFixedWindowLimiter/not global******************************
            /*       builder.Services.AddRateLimiter(options =>
                   {
                       options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                       options.AddFixedWindowLimiter("FixedWindow", x =>
                       {
                           x.PermitLimit = 3;
                           x.QueueLimit = 0;
                           x.Window = TimeSpan.FromSeconds(20);
                           x.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                           x.AutoReplenishment = true;
                       });
                   });*/

            //*******************AddFixedWindowLimiter/global**********************
            /*            builder.Services.AddRateLimiter(options =>
                        {
                            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                           RateLimitPartition.GetFixedWindowLimiter(
                               partitionKey: "ConcurrencyLimiter",
                               factory: x => new FixedWindowRateLimiterOptions
                               {
                                   PermitLimit = 4,
                                   Window = TimeSpan.FromSeconds(20),
                                   QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                   QueueLimit = 0,
                                   AutoReplenishment = true
                               }));
                        });*/

            //***********************SlidingWindowLimiter*****************************
            /*  builder.Services.AddRateLimiter(options =>
              {
                  options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                  options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                 RateLimitPartition.GetSlidingWindowLimiter(
                     partitionKey: "SlidingWindowLimiter",
                     factory: x => new SlidingWindowRateLimiterOptions
                     {
                         PermitLimit = 8,
                         Window = TimeSpan.FromSeconds(30),
                         QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                         QueueLimit = 0,
                         AutoReplenishment = true,
                         SegmentsPerWindow = 3
                     }));

              });*/

            /*   System.Timers.Timer timer = new System.Timers.Timer();
               timer.Elapsed += Timer_Elapsed;
               timer.Interval = 1000;
               timer.Start();*/

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            /*     app.Use((context, next) =>
                 {
                     Console.WriteLine("****************Request is comming *****************");
                     return next(context);
                 });*/

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
           /* app.UseCors(options =>
            {
                options.WithOrigins("https://online.pdp.uz");
            });*/
            app.UseResponseCaching(); // add respone caching middleware
            app.UseOutputCache();// add output caching middleware
            app.UseETagMiddleware(); // CustomMiddleware for using ETag
            //app.UseRateLimiter();//Using Rate Limiters
            app.MapControllers();
         

            app.Run();
        }

        /*       private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
               {
                   Console.WriteLine(e.SignalTime);
               }*/
    }
}
