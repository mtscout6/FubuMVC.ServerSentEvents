namespace FubuMVC.ServerSentEvents.Demo.EventStreams.Default
{
    public class DefaultStreamType : Topic
    {
        public override bool Equals(object obj)
        {
            return obj as DefaultStreamType != null;
        }

        public bool Equals(DefaultStreamType other)
        {
            return other != null;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}