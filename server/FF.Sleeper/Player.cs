using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Sleeper
{
    public class Player
    {
        public string position { get; set; }
        public string team { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int? age { get; set; }
        public int? depth_chart_order { get; set; }
        public int? years_exp { get; set; }
        public int? rotoworld_id { get; set; }
    }
}
