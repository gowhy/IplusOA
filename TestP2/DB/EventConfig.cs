using System;
using System.Collections.Generic;
using System.Reflection;

using QDT.Common.Events;

namespace QDT.Core.MSData
{
    public class EventConfig
    {
        public static readonly EventConfig Instance = new EventConfig();

        public IEventDispatcher EventDispatcher { get; private set; }

        public Func<IUnitOfWork> UnitOfWorkFactory { get; private set; }

        private EventConfig()
        {
        }

        public static void Configure(Action<EventConfig> action)
        {
            Check.Argument.IsNotNull(action, "action");
            action(Instance);
        }

        public EventConfig UsingUnitOfWorkFactory(Func<IUnitOfWork> unitOfWorkFactory)
        {
            Check.Argument.IsNotNull(unitOfWorkFactory, "unitOfWorkFactory");
            UnitOfWorkFactory = unitOfWorkFactory;
            return this;
        }

        public EventConfig UsingDefaultEventDispatcher(params Assembly[] handlerAssemblies)
        {
            return UsingDefaultEventDispatcher(handlerAssemblies as IEnumerable<Assembly>);
        }

        public EventConfig UsingDefaultEventDispatcher(IEnumerable<Assembly> handlerAssemblies)
        {
            Check.Argument.IsNotNull(handlerAssemblies, "handlerAssemblies");

            var registry = new DefaultEventHandlerRegistry();

            foreach (var asm in handlerAssemblies)
            {
                registry.RegisterHandlers(asm);
            }

          //  EventDispatcher = new DefaultEventDispatcher(registry);

            return this;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            if (UnitOfWorkFactory == null)
                throw new InvalidOperationException("Please register unit of work factory first.");

            var unitOfWork = UnitOfWorkFactory();

            if (unitOfWork == null)
                throw new InvalidOperationException("Unit of work factory returns null.");

            return unitOfWork;
        }
    }
}
