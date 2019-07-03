using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Application;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Configuration;
using System.IO;
using Adapters;
using Domain.Adapters;
using System;
using System.Threading.Tasks;

namespace Sub
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var adapterConfiguration = new AdapterConfiguration();
            configuration.Bind("AdapterConfiguration", adapterConfiguration);

            var applicationConfiguration = new ApplicationConfiguration();
            configuration.Bind("ApplicationConfiguration", applicationConfiguration);

            var serviceProvider = new ServiceCollection()
               .AddAdapter(adapterConfiguration)
               .AddApplication(applicationConfiguration)
               .AddSingleton<ISubscriberService, SubscriberService>()
               .AddSingleton<IGooglePubSubAdapter, GooglePubSubAdapter>()
               .AddLogging()
               .BuildServiceProvider();

            try
            {
                var topicName = new TopicName(applicationConfiguration.ProjectId, applicationConfiguration.TopicId);
                var subscriberService = (ISubscriberService)serviceProvider.GetService(typeof(ISubscriberService));
                 await subscriberService.ShowMessagesForSubscriptionAsync(topicName, applicationConfiguration.ProjectId, applicationConfiguration.SubscriptionId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: {0}", e.Message);
            }
        }
    }
}
