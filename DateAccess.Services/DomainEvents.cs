using System;
using System.Threading.Tasks;

namespace DateAccess.Services
{
    public interface IDomainEvent { }

    public interface IEventBroker
    {
        Task Raise<T>(T domainEvent) where T :  IDomainEvent;
    }
    public static class DomainEvents
    {
        private static IEventBroker localEventBroker;

        public static void SetEventBrokerStrategy(IEventBroker eventBroker)
        {
            localEventBroker = eventBroker;
        }

        public static Task Raise<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
        {
            var eventBroker = localEventBroker;
            if(eventBroker==null)
                throw  new InvalidOperationException("You need to provide an event broker first !");
           return eventBroker.Raise(domainEvent);
        }
    }
}
