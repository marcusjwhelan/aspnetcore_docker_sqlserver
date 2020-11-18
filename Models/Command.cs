using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class Command
    {
        // is a primary key, can specify but lets do it, is not nullable
        [Key]
        public int Id { get; set; }
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