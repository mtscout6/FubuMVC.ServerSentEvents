using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;

namespace FubuMVC.ServerSentEvents.Testing
{
    [TestFixture]
    public class SimpleEventQueueTester
    {
        private EventQueue<FakeTopic> theQueue;

        [SetUp]
        public void SetUp()
        {
            theQueue = new EventQueue<FakeTopic>();
            theQueue.Write(new ServerEvent("2", "2"));
            theQueue.Write(new ServerEvent("1", "1"));
            
            for (int i = 3; i < 8; i++)
            {
                theQueue.Write(new ServerEvent(i.ToString(), i.ToString()));
            }    
        }

        [Test]
        public void when_the_topic_has_no_last_or_initial_event_return_everything_in_order()
        {
            theQueue.FindQueuedEvents(new FakeTopic{LastEventId = null, InitialEventId = null}).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("2", "1", "3", "4", "5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = string.Empty, InitialEventId = string.Empty}).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("2", "1", "3", "4", "5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic{LastEventId = string.Empty, InitialEventId = null}).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("2", "1", "3", "4", "5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = null, InitialEventId = string.Empty}).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("2", "1", "3", "4", "5", "6", "7");
        }

        [Test]
        public void return_nothing_if_the_last_event_id_is_current()
        {
            theQueue.FindQueuedEvents(new FakeTopic{LastEventId = "7"})
                .Any().ShouldBeFalse();
        }

        [Test]
        public void return_nothing_if_the_initial_event_id_is_current()
        {
            theQueue.FindQueuedEvents(new FakeTopic{InitialEventId = "7"})
                .Any().ShouldBeFalse();
        }

        [Test]
        public void return_everything_if_the_last_event_id_does_not_match()
        {
            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = "random" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("2", "1", "3", "4", "5", "6", "7");
        }

        [Test]
        public void return_everything_if_the_initial_event_id_does_not_match()
        {
            theQueue.FindQueuedEvents(new FakeTopic { InitialEventId = "random" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("2", "1", "3", "4", "5", "6", "7");
        }

        [Test]
        public void return_everything_if_the_topic_is_null()
        {
            theQueue.FindQueuedEvents(null).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("2", "1", "3", "4", "5", "6", "7");
        }

        [Test]
        public void return_everything_after_last_event_id_if_initial_event_id_comes_first()
        {
            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = "4", InitialEventId = "2"}).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("5", "6", "7");
        }

        [Test]
        public void return_everything_after_initial_event_id_if_last_event_id_comes_first()
        {
            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = "2", InitialEventId = "4"}).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("5", "6", "7");
        }

        [Test]
        public void return_nothing_if_there_are_no_events()
        {
            theQueue.AllEvents.Clear();

            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = null })
                .Any().ShouldBeFalse();
        }

        [Test]
        public void return_everything_following_if_the_last_topic_matches()
        {
            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = "1" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("3", "4", "5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = "3" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("4", "5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = "4" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic { LastEventId = "6" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("7");
        }

        [Test]
        public void return_everything_following_if_the_initial_topic_matches()
        {
            theQueue.FindQueuedEvents(new FakeTopic { InitialEventId = "1" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("3", "4", "5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic { InitialEventId = "3" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("4", "5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic { InitialEventId = "4" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("5", "6", "7");

            theQueue.FindQueuedEvents(new FakeTopic { InitialEventId = "6" }).Select(x => x.Id)
                .ShouldHaveTheSameElementsAs("7");
        }


    }
}