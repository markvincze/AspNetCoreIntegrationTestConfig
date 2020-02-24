using AspNetCoreIntegrationTestConfig.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace AspNetCoreIntegrationTestConfig.IntegrationTests
{
    public class DemoWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services => 
            {
                services.Configure<DemoOptions>(opts =>
                {
                    opts.OptionsConfigProperty = "OptionsConfigProperty_OverriddenInCustomFactory";
                });
            });
        }
    }

    public class OverrideOptionsConfigPropertyWithCustomFactoryTest
    {
        private readonly DemoWebApplicationFactory<Startup> factory = new DemoWebApplicationFactory<Startup>();

        [Fact]
        public async Task OverriddenOptionsConfigValueReceived()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("/demo");

            var responseContent = await response.Content.ReadAsStringAsync();
            var optionsConfigValue = JObject.Parse(responseContent)["optionsConfigProperty"].Value<string>();

            Assert.Equal("OptionsConfigProperty_OverriddenInCustomFactory", optionsConfigValue);
        }
    }
}
