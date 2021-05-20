using System.ComponentModel.DataAnnotations;

namespace SecurePrivacyAPI.Models
{
    public class PersonDto
    {
        [Required]
        public string Name { get; set; }

        [Required, Range(18, int.MaxValue, ErrorMessage = "You must be over 18 to use this API")]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; }
    }
}