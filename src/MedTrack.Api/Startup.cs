using System;
using System.Text;
using MedTrack.Api.Data;
using MedTrack.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace MedTrack.Api
{
    public class Startup
    {
        private const string DevCorsPolicy = "LocalAngular";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<MedTrackContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MedTrack")));

            services.AddCors(options =>
            {
                options.AddPolicy(DevCorsPolicy, policy =>
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();

            var jwtSection = Configuration.GetSection("Jwt");
            services.Configure<JwtSettings>(jwtSection);
            var jwt = jwtSection.Get<JwtSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwt.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwt.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(DevCorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
