namespace FF.Backend.Repositories.Framework
{
    public class UnitOfWork : IUnitOfWork
    {

        protected readonly Contexts.FFContext Database;

        public UnitOfWork(Contexts.FFContext database)
        {
            Database = database;
        }

        public void CommitChanges()
        {
            Database.SaveChanges();
        }
    }
}
