namespace FubuMVC.ServerSentEvents.Demo.EventStreams.CustomQueue
{
    public class EventStateMachineFactory<T> : IEventQueueFactory<T> where T : Topic
    {
        public IEventQueue<T> BuildFor(T topic)
        {
            return new EventStateMachine<T>();
        }
    }
}