using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace wSiteMvc2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //policy para securizar los controllers que solo permiten el acceso a admin
            services.AddAuthorization(options =>
            {
                options.AddPolicy("policyadmin", policyAdmin =>
                {
                    policyAdmin.RequireClaim("role", "admin");
                });                
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //adds the authentication services to DI
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc"; //OpenID Connect scheme
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = this._configuration.GetSection("urlsConfiguration")["identityServer"]; //Identity Server endpoint
                options.RequireHttpsMetadata = false;
                options.ClientId = "mvc2";
                options.SaveTokens = true; //to persist the tokens from IdentityServer in the cookie                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //to ensure the authentication services execute on each request
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
