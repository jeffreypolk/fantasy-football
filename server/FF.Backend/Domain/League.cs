using System.Collections.Generic;

namespace FF.Backend.Domain
{
    public class League: BaseEntity
    {
        public string Name { get; set; }
        public string BylawsUrl { get; set; }
        public string VotingUrl { get; set; }
        public int Established { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Manager> Managers { get; set; }

        public League()
        {
            Teams = new HashSet<Team>();
            Managers = new HashSet<Manager>();
        }
    }
}
