using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Backend.Services.DTO.Stats
{
    public class Money
    {
        public string Name { get; set; }
        public decimal MoneyWon { get; set; }
        public decimal BuyIn { get; set; }
        public int Years { get; set; }
    }
}
