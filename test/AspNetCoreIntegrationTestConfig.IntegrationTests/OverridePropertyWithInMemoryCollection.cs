using AspNetCoreIntegrationTestConfig.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreIntegrationTestConfig.IntegrationTests
{
    public class OverridePropertyWithInMemoryCollection
    {
        private readonly WebApplicationFactory<Startup> factory;

        public OverridePropertyWithInMemoryCollection()
        {
            factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        configBuilder.AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                ["RawConfigProperty"] = "RawConfigProperty_OverriddenWithWithInMemoryCollection"
                            });
                    });
                });
        }

        [Fact]
        public async Task EnvVarOverriddenConfigValueReceived()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("/demo");

            var responseContent = await response.Content.ReadAsStringAsync();
            var optionsConfigValue = JObject.Parse(responseContent)["rawConfigProperty"].Value<string>();

            Assert.Equal("RawConfigProperty_OverriddenWithWithInMemoryCollection", optionsConfigValue);
        }
    }
}
