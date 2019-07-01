using Domain.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using System;
using System.Threading.Tasks;

namespace Application
{
    public class PublishService : IPublishService
    {
        public Task CreateTopic(string topic)
        {
            throw new NotImplementedException();
        }

        public async Task PublishAsync(TopicName topicName, string message)
        {
            var credential = GoogleCredential.FromFile(@"C:\Users\woliveiral\source\repos\PubGoogleCloud\netcore-c9b67cd02535.json");
            var createSettings = new PublisherClient.ClientCreationSettings(
                credentials: credential.ToChannelCredentials());

            var publisher = await PublisherClient.CreateAsync(topicName,
                clientCreationSettings: createSettings);

            string messageId = await publisher.PublishAsync(message);
            await publisher.ShutdownAsync(TimeSpan.FromSeconds(50000));

        }
    }
}
