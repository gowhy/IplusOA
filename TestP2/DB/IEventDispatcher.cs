namespace QDT.Core.MSData
{
    public interface IEventDispatcher
    {
        void Dispatch(IDomainEvent evnt, EventDispatchingContext context);
    }
}
