using FF.Backend.Repositories;
using FF.Backend.Repositories.Framework;
using FF.Backend.Results;
using FF.Backend.Services.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FF.Backend.Services
{
    public class LeagueService : BaseService<Domain.League>, ILeagueService
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueService(IUnitOfWork unitOfWork, ILeagueRepository leagueRepository) : base(unitOfWork)
        {
            _leagueRepository = leagueRepository;
        }

        
        public override Result<IEnumerable<Domain.League>> GetAll()
        {
            var ret = new Result<IEnumerable<FF.Backend.Domain.League>>()
            {
                Data = _leagueRepository.Get().OrderBy(x => x.Id).ToList()
            };
            return ret;
        }

        public override Result<Domain.League> GetById(int id)
        {
            var ret = new Result<FF.Backend.Domain.League>()
            {
                Data = _leagueRepository.Get()
                //.Include(m => m.Teams)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefault()
            };

            return ret;
        }

    }
    public interface ILeagueService : IBaseService<Domain.League>
    {
       
    }
}
