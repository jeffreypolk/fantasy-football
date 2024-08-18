using System.Collections.Generic;

namespace FF.Backend.Domain
{
    public class Manager : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public Manager()
        {
            Teams = new HashSet<Team>();
        }
    }
}
