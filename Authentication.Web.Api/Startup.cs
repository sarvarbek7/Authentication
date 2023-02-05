// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------
using Authentication.Web.Api.Brokers.DateTimeBroker;
using Authentication.Web.Api.Brokers.LoggingBroker;
using Authentication.Web.Api.Brokers.StorageBroker;
using Authentication.Web.Api.Brokers.UserManagament;
using Authentication.Web.Api.Models.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Authentication.Web.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public static void Configure(
            IApplicationBuilder applicationBuilder, 
            IWebHostEnvironment webHostEnvironment)
        {
            if(webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
                applicationBuilder.UseSwagger();

                applicationBuilder.UseSwaggerUI(options =>
                    options.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json",
                        name: "Authentication.Web.Api v1"));
            }

            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseRouting();
            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddLogging();
            services.AddDbContext<StorageBroker>();
            AddBrokers(services);
            services.AddDataProtection();

            services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<StorageBroker>()
                .AddDefaultTokenProviders();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo
                    {
                        Title = "Authentication.Web.Api",
                        Version = "v1",
                    }
                    );
            });
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddScoped<IUserManagementBroker, UserManagementBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
        }


    }
}
