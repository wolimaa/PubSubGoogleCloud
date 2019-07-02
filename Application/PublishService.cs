using Adapters;
using Domain.Adapters;
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
        private readonly IGooglePubSubAdapter googlePubSubAdapter;
        public PublishService(IGooglePubSubAdapter googlePubSubAdapter)
        {
            this.googlePubSubAdapter = googlePubSubAdapter ?? throw new ArgumentNullException(nameof(IGooglePubSubAdapter));
        }

        public Task CreateTopic(string topic)
        {
            throw new NotImplementedException();
        }

        public async Task PublishAsync(TopicName topicName, string message)
        {
            var publisher = await googlePubSubAdapter.GetPublisherClientAsync(topicName);
            string messageId = await publisher.PublishAsync(message);
            await publisher.ShutdownAsync(TimeSpan.FromSeconds(50000));

        }
    }
}
