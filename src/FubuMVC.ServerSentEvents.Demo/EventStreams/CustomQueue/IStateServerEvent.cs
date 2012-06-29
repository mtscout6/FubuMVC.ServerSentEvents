using System;

namespace FubuMVC.ServerSentEvents.Demo.EventStreams.CustomQueue
{
    public interface IStateServerEvent : IServerEvent
    {
        Guid StateId { get; }
    }
}