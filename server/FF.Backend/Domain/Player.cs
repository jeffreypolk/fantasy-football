using System.Collections.Generic;

namespace FF.Backend.Domain
{
    public class Player : BaseEntity
    {
        public string Name { get; set; }
        public string NFLTeam { get; set; }
        public string Position { get; set; }
        public int Age { get; set; }
        public int Experience { get; set; }
        public decimal ADP { get; set; }
        public int DepthChart { get; set; }
        public int Year { get; set; }
        public decimal ActualPoints { get; set; }
        public decimal ProjectedPoints { get; set; }

        public virtual ICollection<PlayerTeam> Teams { get; set; }

        public Player()
        {
            Teams = new HashSet<PlayerTeam>();
        }

    }
}
