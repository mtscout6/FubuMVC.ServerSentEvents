using FubuMVC.Core;
using FubuMVC.Core.Assets.Content;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.ServerSentEvents.Demo.EventStreams.CustomQueue;
using FubuMVC.ServerSentEvents.Demo.Framework;
using FubuMVC.ServerSentEvents.Demo.Framework.Transformers;
using FubuMVC.ServerSentEvents.Demo.Handlers;
using FubuMVC.Spark;

namespace FubuMVC.ServerSentEvents.Demo
{
    public class ApplicationFubuRegistry : FubuRegistry
    {
        public ApplicationFubuRegistry()
        {
            Import<HandlerConvention>(c => c.MarkerType<HomeViewModel>());
            
            Services(x =>
            {
                x.SetServiceIfNone<IModelUrlResolver, ModelUrlResolutionCache>();
                x.AddService<ITransformerPolicy>(new JavascriptTransformerPolicy<UrlTransformer>(ActionType.Transformation, ".js"));

                x.SetServiceIfNone<IEventQueueFactory<CurrentStateTopic>, EventStateMachineFactory<CurrentStateTopic>>();
            });

            Import<SparkEngine>();
            Views.TryToAttachWithDefaultConventions();

            Routes.HomeIs<GetHomeHandler>(h => h.Execute());
        }
    }
}