using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApp.Abstractions.Connectivity;
using SimpleChatApp.Connectivity;
using SimpleChatApp.Domain.Options;
using SimpleChatApp.Extensions;
using SimpleChatApp.Middlewares;

namespace SimpleChatApp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionManager, WebSocketConnectionsManager>();
            services.AddChatServices();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<ChatOptions>(options => Configuration.GetSection("Chat").Bind(options));
            services.Configure<WebSocketOptions>(options => Configuration.GetSection("WebSocket").Bind(options));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseWebSockets();
            app.Map("/ws", config => config.UseMiddleware<WebSocketHandlerMiddleware>());
            app.UseMvc();
        }
    }
}