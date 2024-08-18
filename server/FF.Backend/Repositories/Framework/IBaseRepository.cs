using System.Linq;

namespace FF.Backend.Repositories.Framework
{
    public interface IBaseRepository<T>
    {
        T GetById(object id);
        IQueryable<T> Get();
        T Insert(T entity);
        T Update(T entity);
        T Update(T entity, int id);
        void SoftDelete(object id);
        void Delete(T entity);
        void Delete(object id);
    }
}
