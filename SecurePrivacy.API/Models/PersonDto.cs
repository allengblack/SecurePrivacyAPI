using System.ComponentModel.DataAnnotations;

namespace SecurePrivacy.API.Models
{
    public class PersonDto
    {
        [Required]
        public string Name { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Your age cannot be less than 1")]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Category { get; set; }
    }
}