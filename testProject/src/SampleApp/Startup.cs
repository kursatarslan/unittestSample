using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Models;

namespace SampleApp
{
    public class Startup
    {
        private readonly IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters()
                .AddDataAnnotations()
                .AddApiExplorer()
                .AddAuthorization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IProductsRepository, ProductsRepository>();
            if (env.IsEnvironment("Test"))
            {
                services.AddEntityFrameworkInMemoryDatabase();
                services.AddDbContext<SampleAppContext>(
                    options => options.UseInMemoryDatabase("InMemoryDbForTesting"));
            }
            else
            {
                services.AddDbContext<SampleAppContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("SampleAppContext")));
            }

            services.AddCors(options =>
            {
                options.AddPolicy("cors",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200", "http://localhost:7200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }


        public void Configure(IApplicationBuilder app)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsEnvironment("Test"))
            {
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors("cors");
            app.UseMvc();
        }
    }
}