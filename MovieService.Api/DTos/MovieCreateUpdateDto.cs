namespace MovieService.Api.DTos
{
    public class MovieCreateUpdateDto
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public string Actor { get; set; }
        public string Actress { get; set; }
        public string Language { get; set; }
        public int DurationMinutes { get; set; }
        public string? TrailerUrl { get; set; }

        public IFormFile? File { get; set; }
    }
}
