

namespace QDT.Core.MSData
{
    public class EventDispatchingContext
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public bool WasUnitOfWorkCommitted { get; private set; }

        public EventDispatchingContext(IUnitOfWork unitOfWork, bool wasUnitOfWorkCommitted)
        {
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");
            UnitOfWork = unitOfWork;
            WasUnitOfWorkCommitted = wasUnitOfWorkCommitted;
        }
    }
}
