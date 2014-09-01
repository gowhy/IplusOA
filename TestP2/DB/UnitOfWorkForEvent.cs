
using System;
using System.Collections.Generic;
using System.Linq;
//using QDT.Common.Events;

namespace QDT.Core.MSData
{
    public abstract class UnitOfWorkForEvent : IUnitOfWork
    {
        protected IEventDispatcher EventDispatcher { get; private set; }

        protected IList<IDomainEvent> UncommittedEvents { get; private set; }

        protected bool _disposed;

        protected UnitOfWorkForEvent()
            : this(EventConfig.Instance.EventDispatcher)
        {
        }

        ~UnitOfWorkForEvent()
        {
            Dispose(false);
        }

        protected UnitOfWorkForEvent(IEventDispatcher eventDispatcher)
        {
            Check.Argument.IsNotNull(eventDispatcher, "eventDispatcher");
            EventDispatcher = eventDispatcher;
            UncommittedEvents = new List<IDomainEvent>();
        }

        public void ApplyEvent<TEvent>(TEvent evnt)
            where TEvent : IDomainEvent
        {
            Check.Argument.IsNotNull(evnt, "evnt");

            if (_disposed)
                throw new ObjectDisposedException(null, "Unit of work was disposed.");

            UncommittedEvents.Add(evnt);

            EventDispatcher.Dispatch(evnt, new EventDispatchingContext(this, false));
        }

        public virtual int SaveChanges()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot commit a disposed unit of work.");

            int r = DoCommit();

            var events = UncommittedEvents.ToList();
            UncommittedEvents.Clear();

            if (events.Count > 0)
            {
                DispatchPostCommitEvents(events);
            }
            return r;
        }

        private void DispatchPostCommitEvents(IEnumerable<IDomainEvent> events)
        {
            var context = new EventDispatchingContext(this, true);

            foreach (var evnt in events)
            {
                EventDispatcher.Dispatch(evnt, context);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    UncommittedEvents.Clear();
                }

                _disposed = true;
            }
        }

        protected abstract int DoCommit();

        public abstract T GetByID<T>(object id) where T : class;

        public abstract T Add<T>(T t) where T : class;

        public abstract void Update<T>(T t) where T : class;

        public abstract void Delete<T>(T t) where T : class;

        public abstract void Delete<T, TKey>(TKey key) where T : class, IEntityable<TKey>;

        public abstract int Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class;
    }
}
