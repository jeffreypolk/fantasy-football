using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Backend.Services.DTO.Draft
{
    public class Analysis
    {
        public List<AnalysisTeam> Teams = new List<AnalysisTeam>();
    }

    public class AnalysisTeam
    {
        public string Name { get; set; }
        public int ProjectedFinish { get; set; }
        public int ActualFinish { get; set; }
        public int Finish { get; set; }
        public decimal ProjectedPoints { get; set; }
        public decimal ActualPoints { get; set; }

        public List<AnalysisPlayer> ProjectedLineup = new List<AnalysisPlayer>();

        public List<AnalysisPlayer> ActualLineup = new List<AnalysisPlayer>();
    }

    public class AnalysisPlayer
    {
        public string Name { get; set; }
        public string TeamPosition { get; set; }
        public string PlayerPosition { get; set; }
        public decimal Points { get; set; }
        public int Pick { get; set; }
    }
}
