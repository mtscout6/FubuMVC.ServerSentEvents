using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FubuMVC.ServerSentEvents.Demo.EventStreams
{
    public class DemoEventsManager : IDemoEventsManager
    {
        private readonly IList<IEventInvoker> _invokers;
        private Timer _timer;

        public DemoEventsManager(IEnumerable<IEventInvoker> invokers)
        {
            _invokers = invokers.ToList();
            _timer = new Timer(Callback, null, 0, 5000);
        }

        private void Callback(object state)
        {
            _invokers.Each(i => i.Invoke());
        }
    }

    public interface IDemoEventsManager { }
}