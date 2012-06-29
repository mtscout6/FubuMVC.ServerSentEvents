using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuMVC.ServerSentEvents.Demo.EventStreams.CustomQueue
{
    public class EventStateMachine<T> : IEventQueue<T> where T : Topic
    {
        private readonly ServerEventList<IStateServerEvent> _events = new ServerEventList<IStateServerEvent>();

        public IEnumerable<IServerEvent> FindQueuedEvents(T topic)
        {
            return _events.FindQueuedEvents(topic);
        }

        public void Write(params IServerEvent[] events)
        {
            if (events.Any(e => !(e is IStateServerEvent)))
                throw new Exception("This Event Queue can only handle events of type IStateServerEvent");

            var newStateEvents = events.OfType<IStateServerEvent>().Each(@event => _events.AllEvents
                                                                                       .Where(e => e.StateId == @event.StateId)
                                                                                       .ToList()
                                                                                       .Each(e => _events.AllEvents.Remove(e))).ToList();

            _events.Add(newStateEvents);
        }
    }
}