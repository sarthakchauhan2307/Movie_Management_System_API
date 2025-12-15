using System.Text.Json.Serialization;

namespace TheatreMaster.Api.Models
{
    public class Show
    {
        public int ShowId { get; set; }

        public int MovieId { get; set; }   // NO navigation (cross-service)

        public int ScreenId { get; set; }
        [JsonIgnore]
        public Screen? Screen { get; set; } // Navigation

        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }

        public int Price { get; set; }

        public DateTime Created { get; set; } 
        public DateTime Modified { get; set; }
    }
}
