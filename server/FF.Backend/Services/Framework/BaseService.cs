using FF.Backend.Repositories.Framework;
using FF.Backend.Results;
using System.Collections.Generic;

namespace FF.Backend.Services.Framework
{
    public abstract class BaseService<T> : IBaseService<T>
    {
        protected IUnitOfWork UnitOfWork { get; }

        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract Result<IEnumerable<T>> GetAll();
        public abstract Result<T> GetById(int id);
    }

    public interface IBaseService<T>
    {
        Result<IEnumerable<T>> GetAll();
        Result<T> GetById(int id);
    }
}
