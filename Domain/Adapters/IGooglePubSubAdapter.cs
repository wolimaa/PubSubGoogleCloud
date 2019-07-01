using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Adapters
{
    public interface IGooglePubSubAdapter
    {
        Task<PublisherClient> GetPublisherClientAsync(TopicName topicName);

    }
}
