using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pomodole
{
    public interface IApplicationMessagePublisher
    {
        IApplicationMessageEvent ApplicationMessageEvent { get; }
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
        void Register(IApplicationMessageSubscriber subscriber);
    }

    public class ApplicationMessageEvent : IApplicationMessageEvent
    {
        private Action<IApplicationMessage> Events = (IApplicationMessage message) => { };

        public void Register(IApplicationMessageSubscriber subscriber)
        {
            Events += subscriber.Subject;
        }

        public void Trigger(IApplicationMessage message)
        {
            Events(message);
        }
    }
}
