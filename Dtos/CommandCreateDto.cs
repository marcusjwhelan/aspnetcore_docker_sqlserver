using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class CommandCreateDto
    {
        // attributes prevent the 500 internal server error for keys not listed
        // will send
        // 400 bad request
        // errors: Platform: the Platform field is required
        
        // make sure these are not nullable
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        [Required]
        public string Line { get; set; }
        [Required]
        public string Platform { get; set; }
    }
}