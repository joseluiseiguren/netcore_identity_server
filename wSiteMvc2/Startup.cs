using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

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
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("role"); //custom claim

                //estos son todos los eventos que se pueden capturar (una vez que el usuario ya esta logueado)
                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnAuthenticationFailed = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnUserInformationReceived = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnMessageReceived = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnAuthorizationCodeReceived = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnRedirectToIdentityProvider = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnRedirectToIdentityProviderForSignOut = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnRemoteFailure = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnRemoteSignOut = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnTicketReceived = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    },
                    OnTokenResponseReceived = x =>
                    {
                        Debug.WriteLine(x);
                        return Task.FromResult(0);
                    }
                };
            });

            //policy para securizar los controllers que solo permiten el acceso a admin
            services.AddAuthorization(options =>
            {
                options.AddPolicy("policyadmin", policyAdmin =>
                {
                    //solo los roles que sean admin1 o admin2 van a cumplir esta policy
                    policyAdmin.RequireClaim("role", new string[] { "admin1", "admin2" });
                });
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
