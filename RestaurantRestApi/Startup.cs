using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantRestApi.DtoModels;
using RestaurantRestApi.DtoModels.Validators;
using RestaurantRestApi.Entities;
using RestaurantRestApi.Middleware;
using RestaurantRestApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantRestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettinngs = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettinngs);

            //when we wan to use any of those services inside the target service we have to inject the interface them via its constructor
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false; // doesn't require from client using https
                cfg.SaveToken = true; // token should be saved for authentication
                cfg.TokenValidationParameters = new TokenValidationParameters //define validation parameters for token
                {
                    ValidIssuer = authenticationSettinngs.JwtIssuer, //who issue token
                    ValidAudience = authenticationSettinngs.JwtIssuer, //who can use the token --> here is the same with issuer because we doing it for local
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettinngs.JwtKey)),
                };
            });
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<RestaurantDbContext>();
            services.AddScoped<RestaurantSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantRestApi", Version = "v1" });
            });
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder seeder)
        {
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantRestApi v1"));
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
