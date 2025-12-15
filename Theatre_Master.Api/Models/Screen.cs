using System.Text.Json.Serialization;

namespace TheatreMaster.Api.Models
{
    public class Screen
    {
        public int ScreenId { get; set; }

        public int TheatreId { get; set; }
        [JsonIgnore]
        public Theatre? Theatre { get; set; }

        // Screen Info
        public string ScreenName { get; set; }
        public string ScreenType { get; set; }
        public int SeatCapacity { get; set; }

        // Navigation → Shows
        [JsonIgnore]
        public ICollection<Show>? Shows { get; set; }

        // Audit Fields
        public DateTime Created { get; set; } 
        public DateTime Modified { get; set; }
    }
}
