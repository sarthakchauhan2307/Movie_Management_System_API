namespace MovieService.Api.Models
{
    public class Movies
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public string Actor { get; set; }
        public string Actress { get; set; }
        public string Language { get; set; }
        public int DurationMinutes { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
