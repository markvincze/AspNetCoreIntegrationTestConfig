using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AspNetCoreIntegrationTestConfig.Web.Controllers
{
    [Route("[controller]")]
    public class DemoController : Controller
    {
        private readonly IOptions<DemoOptions> demoOptions;

        private readonly IConfiguration configurationRoot;

        public DemoController(IOptions<DemoOptions> demoOptions, IConfiguration configurationRoot)
        {
            this.demoOptions = demoOptions;
            this.configurationRoot = configurationRoot;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(
                new
                {
                    OptionsConfigProperty = demoOptions.Value.OptionsConfigProperty,
                    RawConfigProperty = configurationRoot["RawConfigProperty"]
                });
        }
    }
}
