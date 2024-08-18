using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain = FF.Backend.Domain;

namespace FF.Backend.Services.DTO.Draft
{
    public class Board
    {
        public int Rounds { get; set; }
        public List<domain.Team> Teams { get; set; }
        public List<domain.PlayerTeam> Picks { get; set; }
        public List<List<int>> Layout { get; set; }
        public string BestPick { get; set; }
        public string WorstPick { get; set; }

        public Analysis Analysis { get; set; }
    }
}
