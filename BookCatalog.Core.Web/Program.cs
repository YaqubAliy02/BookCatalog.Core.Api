using Blazored.LocalStorage;
using BookCatalog.Core.Web.Components;
using BookCatalog.Core.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
namespace BookCatalog.Core.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddScoped<BookService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7282") });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddServerSideBlazor()
                .AddHubOptions(options =>
                {
                    options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
                    options.KeepAliveInterval = TimeSpan.FromMinutes(5);
                });

            builder.Services.AddSignalR(hubOptions =>
             {
                 hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
                 hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
             });

            HttpClient httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            builder.Services.AddServerSideBlazor()
             .AddHubOptions(options =>
                {
                    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
                    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
                    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
