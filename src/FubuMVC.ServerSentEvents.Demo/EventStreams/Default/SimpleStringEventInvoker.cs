using System;
using System.Threading;

namespace FubuMVC.ServerSentEvents.Demo.EventStreams.Default
{
    public class SimpleStringEventInvoker : IEventInvoker
    {
        private static int _count;

        private readonly IEventPublisher _publisher;

        public SimpleStringEventInvoker(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Invoke()
        {
            var count = Interlocked.Increment(ref _count);
            _publisher.WriteTo(new DefaultStreamType(),
                new ServerEvent(Guid.NewGuid().ToString(), "Simple String Event " + count)
                {
                    Event="SimpleStringEvent"
                });
        }
    }
}