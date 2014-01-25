using Serenity.Fixtures;

namespace FubuMVC.ServerSentEvents.Storyteller.Fixtures
{
    public class EventsScreenFixture : ScreenFixture
    {
        protected override void beforeRunning()
        {
            Navigation.NavigateToHome();
        }
    }
}