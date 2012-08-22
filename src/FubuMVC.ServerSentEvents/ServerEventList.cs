using System.Collections.Generic;
using System.Linq;
using FubuCore;

namespace FubuMVC.ServerSentEvents
{
    public class ServerEventList<T> where T : IServerEvent
    {
        private readonly IList<T> _events = new List<T>();

        public IList<T> AllEvents
        {
            get { return _events; }
        }

        public void Add(IEnumerable<T> events)
        {
            _events.AddRange(events);
        }

        public void Add(T @event)
        {
            _events.Add(@event);
        }

        public IEnumerable<T> FindQueuedEvents(Topic topic)
        {
            if (topic == null)
                return _events.ToList();

            var ids = new List<string>();

            if (topic.InitialEventId.IsNotEmpty())
                ids.Add(topic.InitialEventId);

            if (topic.LastEventId.IsNotEmpty())
                ids.Add(topic.LastEventId);

            if (!ids.Any())
                return _events.ToList();

            var foundEvents = new List<T>();

            _events.Each(e =>
            {
                if (ids.Any(i => i == e.Id))
                {
                    foundEvents.Clear();
                    return;
                }

                foundEvents.Add(e);
            });

            return foundEvents;
        }
    }
}