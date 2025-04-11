using System;

namespace Cinema.Models
{
    public class Show
    {
        public string ShowID { get; set; }
        public string MovieID { get; set; }
        public string TheaterID { get; set; }
        public DateTime ShowTime { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}
