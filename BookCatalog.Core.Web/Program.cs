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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7282") });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            /*builder.Services.AddHttpClient("AuthorizedClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7282");
            }).AddHttpMessageHandler<AuthorizationMessageHandler>();*/
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
