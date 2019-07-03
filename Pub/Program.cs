using System;
using System.Threading.Tasks;
using System.Threading;
using Pub.Models;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Application;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Configuration;
using System.IO;
using Adapters;
using Domain.Adapters;

namespace Pub
{
    public class Program
    {
        private static Timer timer;

        static void Main(string[] args)
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
               .AddSingleton<IPublishService, PublishService>()
               .AddSingleton<IGooglePubSubAdapter, GooglePubSubAdapter>()
               .AddLogging()
               .BuildServiceProvider();

            var publishMessage = new PublishMessage
            {
                Message = "Teste",
                ProjectId = applicationConfiguration.ProjectId,
                TopicId = applicationConfiguration.TopicId,
                ServiceCollection = serviceProvider
            };

            timer = new Timer(
                callback: new TimerCallback(TimerTaskAsync),
                state: publishMessage as PublishMessage,
                dueTime: 1000,
                period: 2000);

            while (publishMessage.Counter <= 10)
            {
                Task.Delay(1000).Wait();
            }

            timer.Dispose();
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: done.");
        }

        private static async void TimerTaskAsync(object publishMessage)
        {
            PublishMessage publish = publishMessage as PublishMessage;
            var topicName = new TopicName(publish.ProjectId, publish.TopicId);
            var publishService = publish.ServiceCollection.GetService(typeof(IPublishService));
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: starting a new publish. Message: {publish.Message}");
            Interlocked.Increment(ref publish.Counter);
            await publishService.PublishAsync(topicName, publish.Message);
        }
    }
}
