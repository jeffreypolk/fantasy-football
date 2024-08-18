namespace FF.Backend.Repositories.Framework
{
    public interface IUnitOfWork
    {
        void CommitChanges();
    }
}
