using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Members.OpinionBar.Web.Startup))]
namespace Members.OpinionBar.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
