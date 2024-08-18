using FF.Backend.Contexts;
using FF.Backend.Domain;
using FF.Backend.Repositories.Framework;
using FF.Backend.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;

namespace FF.Backend.Repositories
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        private readonly IDistributedCache _cache;

        public PlayerRepository(FFContext database, IDistributedCache cache) : base(database)
        {
            _cache = cache;
        }
    }

    public interface IPlayerRepository : IBaseRepository<Player>
    { 
    }
}
