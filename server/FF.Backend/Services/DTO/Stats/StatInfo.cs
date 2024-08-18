using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Backend.Services.DTO.Stats
{
    public class StatInfo
    {
        public List<Move> Moves { get; set; } = new List<Move>();
        public List<Playoff> Playoffs { get; set; } = new List<Playoff>();
        public List<PlayoffMiss> PlayoffMisses { get; set; } = new List<PlayoffMiss>();
        public List<PointsAgainst> PointsAgainst { get; set; } = new List<PointsAgainst>();
        public List<PointsFor> PointsFor { get; set; } = new List<PointsFor>();
        public List<Record> Records { get; set; } = new List<Record>();
        public List<Ring> Rings { get; set; } = new List<Ring>();
        public List<Money> Money { get; set; } = new List<Money>();
        public List<BFL> BFLs { get; set; } = new List<BFL>();
        public List<DoubleDigitWin> DoubleDigitWins { get; set; } = new List<DoubleDigitWin>();
        public List<DoubleDigitLoss> DoubleDigitLosses { get; set; } = new List<DoubleDigitLoss>();
    }
}
