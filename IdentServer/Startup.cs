using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentServer
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
            //read in-memory configuration
            var inMemory = new Config(this._configuration);            

            services.AddMvc();

            services.AddIdentityServer()

                //Creates temporary key material for signing tokens (tempkey.rsa)
                //This might be useful to get started, but needs to be replaced by some persistent key material for production scenarios
                .AddDeveloperSigningCredential()

                //lista de identity resources (in-memory) a proteger
                .AddInMemoryIdentityResources(inMemory.GetIdentityResources())

                //lista de recursos (in-memory) a proteger
                .AddInMemoryApiResources(inMemory.GetApiResources())

                //lista de clientes (in-memory), con user/password/allowed recources to consume
                .AddInMemoryClients(inMemory.GetClients())

                //lista de usuarios (in-memory)
                .AddTestUsers(inMemory.GetUsers());                

            //ejemplo de como leer una cadena de conexion a la base de datos, del archivo de configuracion "appsettings.json"
            var databaseConnectionString = this._configuration.GetConnectionString("users_and_clients_database");
            this._logger.LogDebug("Connection String from appsettings.json: " + databaseConnectionString );

            //log startup parameters
            this._logger.LogInformation("urlsConfiguration:redirectWebSite1: " + this._configuration.GetSection("urlsConfiguration")["redirectWebSite1"]);
            this._logger.LogInformation("urlsConfiguration:logoutWebSite1: " + this._configuration.GetSection("urlsConfiguration")["logoutWebSite1"]);
            this._logger.LogInformation("urlsConfiguration:redirectWebSite2: " + this._configuration.GetSection("urlsConfiguration")["redirectWebSite2"]);
            this._logger.LogInformation("urlsConfiguration:logoutWebSite2: " + this._configuration.GetSection("urlsConfiguration")["logoutWebSite2"]);
            this._logger.LogInformation("Logging:LogLevel:Default: " + this._configuration.GetSection("Logging:LogLevel")["Default"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            this._logger.LogDebug("Enviroment: " + env.EnvironmentName);

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
