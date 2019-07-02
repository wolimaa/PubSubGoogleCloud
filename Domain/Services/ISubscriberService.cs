using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ISubscriberService
    {
        Task ShowMessagesForSubscriptionAsync(TopicName topicName, string projectId, string subscriptionId);

    }
}
