using basic_authentication_api.Domain;
using basic_authentication_api.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic_authentication_api.Helpers
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContextPool<AppDbContext>(o =>
			{
				o.UseSqlServer(configuration.GetConnectionString("API_Database"));
			});
			services.AddScoped<IUserRepository, UserRepository>();

			return services;
		}

		public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			// configure strongly typed settings objects
			var appSettingsSection = configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);

			// configure jwt authentication
			var appSettings = appSettingsSection.Get<AppSettings>();

			// should this use utf8?
			var key = Encoding.ASCII.GetBytes(appSettings.JwtSecret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

			return services;
		}
	}
}
