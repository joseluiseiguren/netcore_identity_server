using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace IdentServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIdentityServer()

                //Creates temporary key material for signing tokens (tempkey.rsa)
                //This might be useful to get started, but needs to be replaced by some persistent key material for production scenarios
                .AddDeveloperSigningCredential()

                //lista de identity resources (in-memory) a proteger
                .AddInMemoryIdentityResources(Config.GetIdentityResources())

                //lista de recursos (in-memory) a proteger
                .AddInMemoryApiResources(Config.GetApiResources())

                //lista de clientes (in-memory), con user/password/allowed recources to consume
                .AddInMemoryClients(Config.GetClients())

                //lista de usuarios (in-memory)
                .AddTestUsers(Config.GetUsers());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            //for login UI
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
