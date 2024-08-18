using FF.Backend.Repositories;
using FF.Backend.Repositories.Framework;
using FF.Backend.Results;
using FF.Backend.Services.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FF.Backend.Services
{
    public class TeamService : BaseService<Domain.Team>, ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(IUnitOfWork unitOfWork, ITeamRepository TeamRepository) : base(unitOfWork)
        {
            _teamRepository = TeamRepository;
        }

        public override Result<IEnumerable<Domain.Team>> GetAll()
        {
            var ret = new Result<IEnumerable<FF.Backend.Domain.Team>>()
            {
                Data = _teamRepository.Get().OrderBy(x => x.Name).ToList()
            };
            return ret;
        }

        public override Result<Domain.Team> GetById(int id)
        {
            var ret = new Result<Domain.Team>()
            {
                Data = _teamRepository.Get()
                .Include(t => t.Manager)
                .Include(t => t.Players).ThenInclude(p => p.Player)
                .Where(t => t.Id.Equals(id))
                .FirstOrDefault()
            };

            foreach(var p in ret.Data.Players)
            {
                p.Team = null;
                p.Player.Teams.Clear();
            }
            return ret;
        }


        public Result<IEnumerable<Domain.Team>> GetByLeagueYear(int leagueId, int year)
        {
            var ret = new Result<IEnumerable<Domain.Team>>()
            {
                Data = _teamRepository.Get()
                .Include(t => t.Manager)
                .Where(t => t.LeagueId == leagueId && t.Year == year)
                .OrderBy(t => t.Name)
                .ToList()
            };

            return ret;

        }
    }
    public interface ITeamService : IBaseService<Domain.Team>
    {
        Result<IEnumerable<Domain.Team>> GetByLeagueYear(int leagueId, int year);
    }
}
