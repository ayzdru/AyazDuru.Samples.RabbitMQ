using AyazDuru.Samples.RabbitMQ.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyazDuru.Samples.RabbitMQ.Consumer
{   
    public class MassTransitMessageConsumer : IConsumer<Message>
    {       
        public Task Consume(ConsumeContext<Message> context)
        {
            Console.WriteLine($" [MassTransit] {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
