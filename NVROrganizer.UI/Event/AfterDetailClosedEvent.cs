using Prism.Events;

namespace NvrOrganizer.UI.Event
{
    public class AfterDetailClosedEvent : PubSubEvent<AfterDetailClosedEventArgs>
    {
    }

    public class AfterDetailClosedEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
