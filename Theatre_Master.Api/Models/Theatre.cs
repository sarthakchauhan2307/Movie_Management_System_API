using System.Text.Json.Serialization;

namespace TheatreMaster.Api.Models
{
    public class Theatre
    {
        public int TheatreId { get; set; }
        public string TheatreName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }     
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        [JsonIgnore]
        public ICollection<Screen>? Screens { get; set; }
    }
}
