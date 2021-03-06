using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.Compression;

namespace FubuMVC.ServerSentEvents
{
    [DoNotCompress]
    public class ChannelWriter<T> where T : Topic
    {
        private readonly IClientConnectivity _connectivity;
        private readonly IServerEventWriter _writer;
        private readonly ITopicChannelCache _cache;
        private readonly IChannelInitializer<T> _channelInitializer;
        private IChannel<T> _channel;
        private T _topic;

        private TaskCompletionSource<bool> _liveConnection;

        public ChannelWriter(IClientConnectivity connectivity, IServerEventWriter writer, ITopicChannelCache cache, IChannelInitializer<T> channelInitializer)
        {
            _connectivity = connectivity;
            _writer = writer;
            _cache = cache;
            _channelInitializer = channelInitializer;
        }

        public Task Write(T topic)
        {
            return Task.Factory.StartNew(() =>
            {
                _topic = topic;
                ITopicChannel<T> topicChannel;

                if (!_cache.TryGetChannelFor(_topic, out topicChannel))
                    return;

                _channel = topicChannel.Channel;

                WriteEvents(_channelInitializer.GetInitializationEvents(topic));

                _liveConnection = new TaskCompletionSource<bool>(TaskCreationOptions.AttachedToParent);
                FindEvents();
            }, TaskCreationOptions.AttachedToParent);
        }

        public void FindEvents()
        {
            if (!_connectivity.IsClientConnected() || !_channel.IsConnected())
            {
                _liveConnection.SetResult(false);
                return;
            }

            var task = _channel.FindEvents(_topic);

            OnFaulted(task);
            HandleFoundEvents(task);
        }

        private void HandleFoundEvents(Task<IEnumerable<IServerEvent>> task)
        {
            var continuation = task.ContinueWith(x =>
            {
                if (!_connectivity.IsClientConnected())
                {
                    _liveConnection.SetResult(false);
                    return;
                }

                WriteEvents(x.Result);
                FindEvents();
            }, TaskContinuationOptions.NotOnFaulted); // Intentionally not attached to parent to prevent stack overflow exceptions.

            OnFaulted(continuation);
        }

        private void WriteEvents(IEnumerable<IServerEvent> events)
        {
            var lastSuccessfulMessage = events
                .TakeWhile(y => _writer.Write(y))
                .LastOrDefault();

            if (lastSuccessfulMessage != null)
            {
                _topic.LastEventId = lastSuccessfulMessage.Id;
            }
        }

        private void OnFaulted(Task task)
        {
            task.ContinueWith(x =>
            {
                try
                {
                    var aggregateException = x.Exception.Flatten();
                    _liveConnection.SetException(aggregateException.InnerExceptions);
                }
                finally
                {
                    _liveConnection.SetResult(false);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}