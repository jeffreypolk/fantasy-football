using System.Collections.Generic;

namespace FF.Backend.Domain
{
    public class PlayerTeam: BaseEntity
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public int Round { get; set; }
        public int Overall { get; set; }
        public bool IsKeeper { get; set; }
        public virtual Team Team { get; set; }
        public virtual Player Player { get; set; }

    }
}
