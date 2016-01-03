using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pomodole
{
    public interface IApplicationMessagePublisher
    {
        IApplicationMessageEvent Messenger { get; }
    }

    public interface IApplicationMessageSubscriber
    {
        Action<IApplicationMessage> Subject { get; }
    }

    public interface IApplicationMessageUser : IApplicationMessagePublisher, IApplicationMessageSubscriber
    {

    }

    public interface IApplicationMessageEvent
    {
        void Trigger(IApplicationMessage message);
        void Subscribe(IApplicationMessageSubscriber subscriber);
        void UnSubscribe(IApplicationMessageSubscriber subscriber);
    }

    public class ApplicationMessageEvent : IApplicationMessageEvent
    {
        private Action<IApplicationMessage> Events = (IApplicationMessage message) => { };

        public void Subscribe(IApplicationMessageSubscriber subscriber)
        {
            Events += subscriber.Subject;
        }

        public void Trigger(IApplicationMessage message)
        {
            Events(message);
        }

        public void UnSubscribe(IApplicationMessageSubscriber subscriber)
        {
            Events -= subscriber.Subject;
        }
    }
}
