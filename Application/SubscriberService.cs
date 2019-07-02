using Domain.Adapters;
using Domain.Services;
using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IGooglePubSubAdapter googlePubSubAdapter;

        public SubscriberService(IGooglePubSubAdapter googlePubSubAdapter)
        {
            this.googlePubSubAdapter = googlePubSubAdapter ?? throw new ArgumentNullException(nameof(IGooglePubSubAdapter));
        }
        public async Task ShowMessagesForSubscriptionAsync(TopicName topicName, string projectId, string subscriptionId)
        {
            var subscriptionName = new SubscriptionName(projectId, subscriptionId);
            var subscriber = await googlePubSubAdapter.GetSubscriberClientAsync(topicName, projectId, subscriptionId);

            try
            {
                await subscriber.StartAsync((msg, cancellationToken) =>
                {
                    Console.WriteLine($"Received message {msg.MessageId} published at {msg.PublishTime.ToDateTime()}");
                    Console.WriteLine($"Text: '{msg.Data.ToStringUtf8()}'");                  
                    return Task.FromResult(SubscriberClient.Reply.Ack);
                });

            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: {0}", e.Message);

            }
        }
    }
}
