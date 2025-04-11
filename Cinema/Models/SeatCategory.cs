using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class SeatCategory
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public decimal PriceMultiplier { get; set; }
    }
}
