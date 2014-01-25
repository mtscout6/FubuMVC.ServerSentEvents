using FubuMVC.Core;

namespace FubuMVC.ServerSentEvents.Storyteller.Application.Endpoints.SimpleTopic
{
    public class TriggerSimpleEventEndpoint
    {
        private readonly IEventPublisher _eventPublisher;

        public TriggerSimpleEventEndpoint(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        [UrlPattern("simple-event")]
        public void Post(TriggerSimpleEventInputModel input)
        {
            _eventPublisher.WriteTo(new SimpleTopic(), new ServerEvent(input.EventId, input));
        }
    }

    public class TriggerSimpleEventInputModel
    {
        public string EventId { get; set; }
        public string Data { get; set; }
    }
}