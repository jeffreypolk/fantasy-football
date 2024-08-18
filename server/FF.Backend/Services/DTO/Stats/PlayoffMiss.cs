using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Backend.Services.DTO.Stats
{
    public class PlayoffMiss
    {
        public string Name { get; set; }
        public int PlayoffMisses { get; set; }
        public int Years { get; set; }
        public decimal PlayoffMissPercentage { get; set; }
    }
}
