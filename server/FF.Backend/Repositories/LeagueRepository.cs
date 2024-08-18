using FF.Backend.Contexts;
using FF.Backend.Domain;
using FF.Backend.Repositories.Framework;
using FF.Backend.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;

namespace FF.Backend.Repositories
{
    public class LeagueRepository : BaseRepository<League>, ILeagueRepository
    {
        private readonly IDistributedCache _cache;

        public LeagueRepository(FFContext database, IDistributedCache cache) : base(database)
        {
            _cache = cache;
        }
    }

    public interface ILeagueRepository : IBaseRepository<League>
    {
    }
}
