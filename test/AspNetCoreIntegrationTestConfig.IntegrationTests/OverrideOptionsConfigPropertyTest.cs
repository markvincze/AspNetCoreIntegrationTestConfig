using AspNetCoreIntegrationTestConfig.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreIntegrationTestConfig.IntegrationTests
{
    public class OverrideOptionsConfigPropertyTest
    {
        private readonly WebApplicationFactory<Startup> factory;

        public OverrideOptionsConfigPropertyTest()
        {
            factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.Configure<DemoOptions>(opts =>
                        {
                            opts.OptionsConfigProperty = "OptionsConfigProperty_Overridden";
                        });
                    });
                });
        }

        [Fact]
        public async Task OverriddenOptionsConfigValueReceived()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("/demo");

            var responseContent = await response.Content.ReadAsStringAsync();
            var optionsConfigValue = JObject.Parse(responseContent)["optionsConfigProperty"].Value<string>();

            Assert.Equal("OptionsConfigProperty_Overridden", optionsConfigValue);
        }
    }
}
