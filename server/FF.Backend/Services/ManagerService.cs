using FF.Backend.Repositories;
using FF.Backend.Repositories.Framework;
using FF.Backend.Results;
using FF.Backend.Services.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FF.Backend.Services
{
    public class ManagerService : BaseService<Domain.Manager>, IManagerService
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IUnitOfWork unitOfWork, IManagerRepository managerRepository) : base(unitOfWork)
        {
            _managerRepository = managerRepository;
        }

        
        public override Result<IEnumerable<Domain.Manager>> GetAll()
        {
            var ret = new Result<IEnumerable<FF.Backend.Domain.Manager>>()
            {
                Data = _managerRepository.Get().OrderBy(x => x.Name).ToList()
            };
            return ret;
        }

        public override Result<Domain.Manager> GetById(int id)
        {
            var ret = new Result<FF.Backend.Domain.Manager>()
            {
                Data = _managerRepository.Get()
                .Include(m => m.Teams)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefault()
            };

            foreach(var team in ret.Data.Teams)
            {
                team.Manager = null;
                team.League = null;
            }
            return ret;
        }

    }
    public interface IManagerService : IBaseService<Domain.Manager>
    {
       
    }
}
