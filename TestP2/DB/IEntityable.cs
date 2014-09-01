namespace QDT.Core.MSData
{
    public interface IEntityable<TKey> {
        TKey Id { get; }
        void Save();
        void Update();
        void Delete();
        bool IsValidate();
    }

}
