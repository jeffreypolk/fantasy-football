using FF.Backend.Repositories;
using FF.Backend.Repositories.Framework;
using FF.Backend.Results;
using FF.Backend.Services.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FF.Backend.Services
{
    public class PlayerService : BaseService<Domain.Player>, IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IUnitOfWork unitOfWork, IPlayerRepository playerRepository) : base(unitOfWork)
        {
            _playerRepository = playerRepository;
        }

        public override Result<IEnumerable<Domain.Player>> GetAll()
        {
            throw new System.NotSupportedException();
        }

        public override Result<Domain.Player> GetById(int id)
        {
            var ret = new Result<Domain.Player>()
            {
                Data = _playerRepository.Get()
                .Where(t => t.Id.Equals(id))
                .FirstOrDefault()
            };

            return ret;
        }


        public Result<IEnumerable<Domain.Player>> GetByYear(int year)
        {
            var ret = new Result<IEnumerable<Domain.Player>>()
            {
                Data = _playerRepository.Get()
                .Where(p => p.Year == year)
                .OrderBy(t => t.Name)
                .ToList()
            };

            return ret;

        }
    }
    public interface IPlayerService : IBaseService<Domain.Player>
    {
        Result<IEnumerable<Domain.Player>> GetByYear(int year);
    }
}
