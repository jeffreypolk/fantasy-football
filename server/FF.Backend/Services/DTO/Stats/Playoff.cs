using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Backend.Services.DTO.Stats
{
    public class Playoff
    {
        public string Name { get; set; }
        public int Playoffs { get; set; }
        public int Years { get; set; }
        public decimal PlayoffPercentage { get; set; }
    }
}
