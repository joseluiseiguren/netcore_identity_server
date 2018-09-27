using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
                       ILogger<Startup> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        //para acceder al archivo de configuracion "appsettings.json"
        public IConfiguration _configuration { get; }

        //para loguear en el logger seteado en el program.cs
        private readonly ILogger<Startup> _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                
                //policy para securizar el endpoint de Employees
                .AddAuthorization(options => options.AddPolicy("protectedScopeEmployee", policy =>
                {
                    policy.RequireClaim("scope", "MyWebAPI.employee");
                }))
                
                //policy para securizar el endpoint de Customers
                .AddAuthorization(options => options.AddPolicy("protectedScopeCustomer", policy =>
                {
                    policy.RequireClaim("scope", "MyWebAPI.customer");                    
                }))
                .AddJsonFormatters();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000"; //url del identity server
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "MyWebAPI"; //identificador de la webapi                    
                });

            this._logger.LogDebug("Finish ConfigureServices...");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //adds the authentication middleware to the pipeline so authentication will be performed automatically on every call into the host.
            app.UseAuthentication();

            app.UseMvc();

            this._logger.LogDebug("Finish Configure...");
        }
    }
}
