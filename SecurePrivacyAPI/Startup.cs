using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SecurePrivacyAPI.Models;
using SecurePrivacyAPI.Services;
using System;

namespace SecurePrivacyAPI
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
            var options = new MongoOptions();
            var section = Configuration.GetSection("Mongo");
            section.Bind(options);
            services.Configure<MongoOptions>(section);

            services.AddScoped<IMongoCollection<Person>>(x => 
            {
                return new MongoClient(options.ConnectionString).GetDatabase(options.DatabaseName).GetCollection<Person>(nameof(Person));
            });

            services.AddAutoMapper(typeof(MapperProfile));
            services.AddScoped<IPersonService, PersonService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Title = "SecurePrivacyAPI", Version = "v1" ,
                    Contact = new OpenApiContact
                    {
                        Name = "Gbolahan Allen",
                        Email = "allengbolahan@gmail.com",
                        Url = new Uri("https://github.com/allengblack")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SecurePrivacyAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
