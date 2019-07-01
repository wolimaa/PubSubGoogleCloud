using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IPublishService
    {
        Task PublishAsync(TopicName topicName, string message);
        Task CreateTopic(string topic);

    }
}
