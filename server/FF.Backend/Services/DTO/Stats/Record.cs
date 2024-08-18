using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Backend.Services.DTO.Stats
{
    public class Record
    {
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public int Finish { get; set; }
        public int Years { get; set; }
        public decimal WinPercentage { get; set; }
    }
}
