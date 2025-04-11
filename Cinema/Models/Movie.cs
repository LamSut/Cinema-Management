using System;

namespace Cinema.Models
{
    public class Movie
    {
        public string MovieID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterURL { get; set; }
        public string Status { get; set; }
    }
}
