using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserService.Api.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }   
        public string? Role { get; set; } 
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }
}
