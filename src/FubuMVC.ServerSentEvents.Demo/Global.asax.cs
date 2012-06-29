using System;
using System.Web;
using FubuMVC.Core;
using FubuMVC.ServerSentEvents.Demo.EventStreams;
using FubuMVC.StructureMap;
using StructureMap;

namespace FubuMVC.ServerSentEvents.Demo
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var container = ObjectFactory.Container;

            FubuApplication
                .For<ApplicationFubuRegistry>()
                .ContainerFacility(() =>
                {
                    container.Configure(x =>
                    {
                        x.Scan(scan =>
                        {
                            scan.TheCallingAssembly();
                            scan.AddAllTypesOf<IEventInvoker>();
                        });
                        x.For<IDemoEventsManager>().Singleton().Use<DemoEventsManager>();
                    });
                    return new StructureMapContainerFacility(container).DoNotInitializeSingletons();
                })
                .Bootstrap();

            container.GetInstance<IDemoEventsManager>();
        }
    }
}