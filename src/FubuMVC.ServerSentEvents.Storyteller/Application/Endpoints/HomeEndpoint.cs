using FubuMVC.Core.Urls;
using HtmlTags;

namespace FubuMVC.ServerSentEvents.Storyteller.Application.Endpoints
{
    public class HomeEndpoint
    {
        private readonly IUrlRegistry _urlRegistry;

        public HomeEndpoint(IUrlRegistry urlRegistry)
        {
            _urlRegistry = urlRegistry;
        }

        public HtmlDocument Index()
        {
            var document = new HtmlDocument
            {
                Title = "Server Sent Events / Storyteller Harness"
            };

            document.Add("h1").Text("Server Sent Events / Storyteller Harness");

            var simpleEventTrigger = new DivTag();
            simpleEventTrigger.Id("simpleEventPost");

            simpleEventTrigger.Children.Add(new HtmlTag("label").Text("Event Id: "));
            simpleEventTrigger.Children.Add(new TextboxTag().AddClasses("event-id"));

            simpleEventTrigger.Children.Add(new HtmlTag("label").Text("Event Data: "));
            simpleEventTrigger.Children.Add(new TextboxTag().AddClasses("event-data"));

            simpleEventTrigger.Children.Add(new HtmlTag("button").Text("Send").Attr("onClick", "simpleEventPost(event, this)"));

            document.Add(simpleEventTrigger);

            document.Add(new DivTag("receivedSimpleEvents"));

            document.ReferenceJavaScriptFile("http://localhost:5500/_content/scripts/jquery-2.0.3.min.js");
            document.ReferenceJavaScriptFile("http://localhost:5500/_content/scripts/server-sent-events.js");

            return document;
        }
    }
}