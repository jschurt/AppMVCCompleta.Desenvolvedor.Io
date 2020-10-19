using AutoMapper;
using DevIO.App.Configurations;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevIO.App
{
    public class Startup
    {

        public IConfiguration _configuration { get; }

        /// <summary>
        /// Constructor personalizado para permitir a existencia de diferentes appsettings por ambiente
        /// </summary>
        /// <param name="hostEnvironment"></param>
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            _configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //DbContext for Entity
            services.AddDbContext<MeuDbContext>(options =>
                options.UseSqlServer(
                    _configuration.GetConnectionString("DefaultConnection")));

            //Add AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //Adicionando minha configuracao do Identity (extension method)
            services.AddIdentityConfiguration(_configuration);

            //Adicionando minha configuracao do MVC (extension method)
            services.AddMvcConfiguration();

            //Resolvendo minhas injecoes de dependencia (extension method)
            services.AddDependencyInjectionConfig();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/erro/500");
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                
                //Adiciona Strict-Transport-Security dentro do header. 
                //Se o usuario tentar forcar uma conexao nao segura, o browser forcara seguir por uma conexao segura.
                //Se nao houver uma conexao segura, ocorrera um erro. (http -> https)
                app.UseHsts();
            }

            //Redireciona a resposta para https (caso a conexao seja feita por http)
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Adicionando Culture/Localization (extension method)
            app.UseGlobalizationConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
