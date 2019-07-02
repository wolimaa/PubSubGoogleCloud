using System;
using System.Collections.Generic;
using System.Text;

namespace Pub.Models
{
    public class PublishMessage
    {
        public string Message;

        public int Counter;
        public string ProjectId { get; set; }
        public string TopicId { get; set; }
        public dynamic ServiceCollection { get; set; }

    }
}
