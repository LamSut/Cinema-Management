using System;

namespace Cinema.Models
{
    public class Ticket
    {
        public string BookingID { get; set; }
        public string Seats { get; set; }
        public string SeatCategory { get; set; }
        public string MovieTitle { get; set; }
        public DateTime ShowTime { get; set; }
        public string TheaterName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
