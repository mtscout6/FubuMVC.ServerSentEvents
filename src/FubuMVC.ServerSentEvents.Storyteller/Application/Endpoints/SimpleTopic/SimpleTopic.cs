namespace FubuMVC.ServerSentEvents.Storyteller.Application.Endpoints.SimpleTopic
{
    public class SimpleTopic : Topic
    {
        public override bool Equals(object obj)
        {
            return Equals(obj as SimpleTopic);
        }

        public bool Equals(SimpleTopic topic)
        {
            return !ReferenceEquals(null, topic);
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}