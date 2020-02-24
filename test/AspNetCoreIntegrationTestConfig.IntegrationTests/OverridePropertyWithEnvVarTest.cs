using AspNetCoreIntegrationTestConfig.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreIntegrationTestConfig.IntegrationTests
{
    public class OverridePropertyWithEnvVarTest : IDisposable
    {
        private readonly WebApplicationFactory<Startup> factory;

        public OverridePropertyWithEnvVarTest()
        {
            Environment.SetEnvironmentVariable("RawConfigProperty", "RawConfigProperty_OverriddenWithEnvVar");
            factory = new WebApplicationFactory<Startup>();
        }

        [Fact]
        public async Task EnvVarOverriddenConfigValueReceived()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("/demo");

            var responseContent = await response.Content.ReadAsStringAsync();
            var optionsConfigValue = JObject.Parse(responseContent)["rawConfigProperty"].Value<string>();

            Assert.Equal("RawConfigProperty_OverriddenWithEnvVar", optionsConfigValue);
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable("RawConfigProperty", "");
            factory?.Dispose();
        }
    }
}
