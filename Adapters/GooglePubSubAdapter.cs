using Domain.Adapters;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Adapters
{
    public class GooglePubSubAdapter : IGooglePubSubAdapter
    {

        private readonly AdapterConfiguration adapterConfiguration;

        public GooglePubSubAdapter(
            AdapterConfiguration adapterConfiguration)
        {
            this.adapterConfiguration = adapterConfiguration ?? throw new ArgumentNullException(nameof(AdapterConfiguration));
        }

        public Task<PublisherClient> GetPublisherClientAsync(TopicName topicName)
        {
            var credential = GoogleCredential.FromFile(adapterConfiguration.GoogleCredentialFile);
            var createSettings = new PublisherClient.ClientCreationSettings(
                credentials: credential.ToChannelCredentials());

            return PublisherClient.CreateAsync(topicName, clientCreationSettings: createSettings);
        }
    }
}
