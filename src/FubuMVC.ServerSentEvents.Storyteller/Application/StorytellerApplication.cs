using FubuMVC.Core;
using FubuMVC.StructureMap;

namespace FubuMVC.ServerSentEvents.Storyteller.Application
{
    public class SseStorytellerApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication.For<SseStorytellerFubuRegistry>().StructureMap();
        }
    }

    public class SseStorytellerFubuRegistry : FubuRegistry
    {
        public SseStorytellerFubuRegistry()
        {
            Actions.IncludeClassesSuffixedWithEndpoint();
            Import<ServerSentEventsExtension>();
        }
    }
}