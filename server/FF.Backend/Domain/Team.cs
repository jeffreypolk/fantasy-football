using System.Collections.Generic;

namespace FF.Backend.Domain
{
    public class Team: BaseEntity
    {
        public int LeagueId { get; set; }
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public int DraftOrder { get; set; }
        public int Year { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public decimal PointsFor { get; set; }
        public decimal PointsAgainst { get; set; }
        public int Finish { get; set; }
        public int Moves { get; set; }
        public bool IsPlayoffs { get; set; }
        public bool IsConsolation { get; set; }
        public bool IsBottom { get; set; }
        public bool IsBFL { get; set; }

        public virtual Manager Manager { get; set; }
        public virtual League League { get; set; }
        public virtual ICollection<PlayerTeam> Players { get; set; }

        public Team()
        {
            Players = new HashSet<PlayerTeam>();
        }
    }
}
