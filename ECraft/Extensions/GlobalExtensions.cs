using Azure;
using ECraft.Contracts.Response;
using ECraft.Domain;
using ECraft.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace ECraft.Extensions
{
    public static class GlobalExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, TokenAuthService>();

            services.ConfigureSwaggerGen(conf =>
            {
                conf.AddSecurityDefinition("AuthDefinition", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Enter the jwt token: ",
                    Type = SecuritySchemeType.Http
                });

                conf.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "AuthDefinition",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
            });

            return services;
        }


        public static ErrorList GetErrorList(this ModelStateDictionary modelState)
        {
            ErrorList result = new ErrorList();

            foreach (var value in modelState.Values)
            {
                var errorCollection = value.Errors;
                foreach (var error in errorCollection)
                {
                    result.AddError(error.GetHashCode().ToString(), error.ErrorMessage);
                }
            }

            return result;
        }

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if(claimsPrincipal == null)
                throw new ArgumentNullException(nameof(claimsPrincipal));


            Claim? userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "uid");

            if (userIdClaim == null)
            {
                throw new Exception("uid claim is not in the claims principal");
            }

            bool parsed = int.TryParse(userIdClaim.Value, out int userId);
            if (parsed)
            {
                return userId;
            }
            else
                throw new Exception("uid claim value is not an integer value");
        }
    }
}
