using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreIntegrationTestConfig.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private readonly IConfiguration configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DemoOptions>(configuration.GetSection("Demo"));

            services.AddRouting();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(opt =>
            {
                opt.MapControllers();
            });
        }
    }
}
