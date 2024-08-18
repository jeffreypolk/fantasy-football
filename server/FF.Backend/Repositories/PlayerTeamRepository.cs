using FF.Backend.Contexts;
using FF.Backend.Domain;
using FF.Backend.Repositories.Framework;
using FF.Backend.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;

namespace FF.Backend.Repositories
{
    public class PlayerTeamRepository : BaseRepository<PlayerTeam>, IPlayerTeamRepository
    {
        private readonly IDistributedCache _cache;

        public PlayerTeamRepository(FFContext database, IDistributedCache cache) : base(database)
        {
            _cache = cache;
        }
    }

    public interface IPlayerTeamRepository : IBaseRepository<PlayerTeam>
    {
    }
}
