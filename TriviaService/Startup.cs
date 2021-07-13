using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Threading.Tasks;


[assembly: OwinStartup(typeof(TriviaService.Startup))]

namespace TriviaService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.Map("/signalr", map =>
            {                
                var hubConfiguration = new Microsoft.AspNet.SignalR.HubConfiguration
                {                    
                    EnableJSONP = true,
                };                
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
