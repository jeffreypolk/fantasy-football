using FF.Backend.Contexts;
using FF.Backend.Domain;
using FF.Backend.Repositories.Framework;
using FF.Backend.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;

namespace FF.Backend.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        private readonly IDistributedCache _cache;

        public TeamRepository(FFContext database, IDistributedCache cache) : base(database)
        {
            _cache = cache;
        }
    }

    public interface ITeamRepository : IBaseRepository<Team>
    { 
    }
}
