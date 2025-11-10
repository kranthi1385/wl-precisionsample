using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Members.PrecisionSample.EndLinks.Startup))]
namespace Members.PrecisionSample.EndLinks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
