using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace DateAccess.Services.Infrastructure
{
    public interface IHandle<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task Handle(TDomainEvent domainEvent);
    }

    public class AutofacEventBroker:IEventBroker
    {
        private readonly IContainer localContainer;

        public AutofacEventBroker(IContainer container)
        {
            localContainer = container;
        }

        public Task Raise<T>(T domainEvent) where T : IDomainEvent
        {
            var handlers = localContainer.Resolve<IEnumerable<IHandle<T>>>();
            return Task.WhenAll(handlers.Select(handler => handler.Handle(domainEvent)).ToArray());
        }
    }
}
