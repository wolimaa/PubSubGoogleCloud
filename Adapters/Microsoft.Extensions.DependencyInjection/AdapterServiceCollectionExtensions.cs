using Adapters;
using Domain.Adapters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdapterServiceCollectionExtensions
    {
        public static IServiceCollection AddAdapter(this IServiceCollection services, AdapterConfiguration adapterConfiguration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (adapterConfiguration == null)
                throw new ArgumentNullException(nameof(adapterConfiguration));

            // Registra a instancia do objeto de configuracoes desta camanda.
            services.AddSingleton(adapterConfiguration);


            // Registra a implementacao do IGooglePubSubAdapter para ser utilizado na camada de aplicacao.
            services.AddScoped<IGooglePubSubAdapter, GooglePubSubAdapter>();

            return services;
        }
    }
}
