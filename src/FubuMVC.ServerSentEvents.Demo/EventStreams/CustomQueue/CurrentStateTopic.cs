namespace FubuMVC.ServerSentEvents.Demo.EventStreams.CustomQueue
{
    public class CurrentStateTopic : Topic
    {
        public override bool Equals(object obj)
        {
            return obj as CurrentStateTopic != null;
        }

        public bool Equals(CurrentStateTopic other)
        {
            return other != null;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}