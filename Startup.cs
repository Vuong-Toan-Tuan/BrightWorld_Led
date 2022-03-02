using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BrightWorld_LED.Startup))]
namespace BrightWorld_LED
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
