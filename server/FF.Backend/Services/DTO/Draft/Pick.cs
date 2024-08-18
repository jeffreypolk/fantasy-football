using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Backend.Services.DTO.Draft
{
    public class Pick
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public int Round { get; set; }
        public int Overall { get; set; }
        public bool IsKeeper { get; set; }
    }
}
