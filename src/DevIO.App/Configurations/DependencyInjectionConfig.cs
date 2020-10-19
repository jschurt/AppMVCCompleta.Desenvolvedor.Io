using DevIO.App.Extensions;
using DevIO.Business.Interfaces;
using DevIO.Business.Notificacoes;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.App.Configurations
{
    /// <summary>
    /// Classe com IServiceCollection extension methods com a configuracao de DI
    /// </summary>
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
        {
            //Adicionando DbContext
            services.AddScoped<MeuDbContext>();
            
            //Adicionando repositories
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            //Adicionando servicos
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IFornecedorService, FornecedorService>();

            //Adicionando meu adapter para validacao de moeda no lado cliente
            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            //Adicionando Notificador de Erro
            services.AddScoped<INotificadorErro, NotificadorErro>();

            return services;

        }

    }

}
