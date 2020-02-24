using AspNetCoreIntegrationTestConfig.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreIntegrationTestConfig.IntegrationTests
{
    public class OverridePropertyWithAppsettingsJson
    {
        private readonly TestServer testServer;

        public OverridePropertyWithAppsettingsJson()
        {
            testServer = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json");
                })
                .UseStartup<Startup>());
        }

        [Fact]
        public async Task OverriddenOptionsConfigValueReceived()
        {
            var client = testServer.CreateClient();

            var response = await client.GetAsync("/demo");

            var responseContent = await response.Content.ReadAsStringAsync();
            var optionsConfigValue = JObject.Parse(responseContent)["rawConfigProperty"].Value<string>();

            Assert.Equal("RawConfigProperty_OverriddenInAppsettingsJson", optionsConfigValue);
        }
    }
}
