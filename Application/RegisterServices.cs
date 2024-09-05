using System.Reflection;
using Application.Abstraction;
using Application.Mappings;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class RegisterServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddLazyCache();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
