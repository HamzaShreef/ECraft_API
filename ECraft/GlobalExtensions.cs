using ECraft.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;

namespace ECraft
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



		public static List<string> GetErrorList(this ModelStateDictionary modelState)
		{
			var errors = modelState.Values.SelectMany(x=>x.Errors.Select(err=>err.ErrorMessage));

			//List<string> list = new List<string>();
			//foreach ( var value in modelState.Values )
			//{
			//	var errorCollection = value.Errors;
			//	foreach ( var error in errorCollection )
			//	{
			//		list.Add(error.ErrorMessage);
			//	}
			//}
			
			return errors.ToList();
		}
	}
}
