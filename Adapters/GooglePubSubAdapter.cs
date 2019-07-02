using Domain.Adapters;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Adapters
{
    public class GooglePubSubAdapter : IGooglePubSubAdapter
    {

        private readonly AdapterConfiguration adapterConfiguration;
        private readonly GoogleCredential credential;
        private readonly PublisherClient.ClientCreationSettings createSettingsPub;
        private readonly SubscriberClient.ClientCreationSettings createSettingsSub;
        public GooglePubSubAdapter(
            AdapterConfiguration adapterConfiguration)
        {
            this.credential = GoogleCredential.FromFile($"{Directory.GetCurrentDirectory()}\\{adapterConfiguration.GoogleCredentialFile}") ?? throw new ArgumentNullException(nameof(AdapterConfiguration));
            this.adapterConfiguration = adapterConfiguration ?? throw new ArgumentNullException(nameof(AdapterConfiguration));

            this.createSettingsPub = new PublisherClient.ClientCreationSettings(
              credentials: credential.ToChannelCredentials());

            this.createSettingsSub = new SubscriberClient.ClientCreationSettings(
              credentials: credential.ToChannelCredentials());
        }

        public Task<PublisherClient> GetPublisherClientAsync(TopicName topicName)
        {
            return PublisherClient.CreateAsync(topicName, clientCreationSettings: createSettingsPub);
        }

        public Task<SubscriberClient> GetSubscriberClientAsync(TopicName topicName, string projectId, string subscriptionId)
        {
            var subscriptionName = new SubscriptionName(projectId, subscriptionId);
            return SubscriberClient.CreateAsync(subscriptionName, clientCreationSettings: createSettingsSub);

        }

    }
}
