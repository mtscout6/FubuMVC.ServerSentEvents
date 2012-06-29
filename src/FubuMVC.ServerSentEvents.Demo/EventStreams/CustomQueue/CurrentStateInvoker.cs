using System;
using System.Collections.Generic;
using FubuCore;

namespace FubuMVC.ServerSentEvents.Demo.EventStreams.CustomQueue
{
    public class CurrentStateInvoker : IEventInvoker
    {
        private readonly List<CurrentState> _states;
        private readonly Random _random = new Random(1);
        private readonly string[] _names = new[]
        {
            "Matt",
            "Andrew",
            "Ryan",
            "Jordan",
            "Mike"
        };

        public CurrentStateInvoker(IServiceLocator serviceLocator)
        {
            _states = new List<CurrentState>();

            for (var i = 0; i < 5; i++)
            {
                _states.Add(serviceLocator.GetInstance<CurrentState>());
            }
        }

        public void Invoke()
        {
            for (var i = 0; i < 2; i++)
            {
                _states[_random.Next(0, _states.Count)].SomeValue++;
                _states[_random.Next(0, _states.Count)].CurrentName = _names[_random.Next(0, _names.Length)];
            }
        }
    }

    public class CurrentState
    {
        private readonly IEventPublisher _eventPublisher;
        private int _someValue;
        private string _currentName;
        private readonly Guid _stateId;

        public int SomeValue
        {
            get { return _someValue; }
            set
            {
                _someValue = value;
                Update();
            }
        }

        public string CurrentName
        {
            get { return _currentName; }
            set 
            {
                _currentName = value;
                Update();
            }
        }

        public CurrentState(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _stateId = Guid.NewGuid();
            Update();
        }

        private void Update()
        {
            _eventPublisher.WriteTo(new CurrentStateTopic(),
                new CurrentStateEvent(new CurrentStateEventData
                {
                    StateId = _stateId,
                    SomeValue = SomeValue,
                    CurrentName = CurrentName
                }));
        }

        private class CurrentStateEventData
        {
            public Guid StateId { get; set; }
            public int SomeValue { get; set; }
            public string CurrentName { get; set; }
        }

        private class CurrentStateEvent : IStateServerEvent
        {
            public string Id { get; private set; }
            public string Event { get { return "CurrentStateChanged"; } }
            public int? Retry { get { return null; } }
            public object Data { get; private set; }

            public Guid StateId { get; private set; }

            public CurrentStateEvent(CurrentStateEventData state)
            {
                Id = Guid.NewGuid().ToString();
                StateId = state.StateId;
                Data = state;
            }
        }
    }
}