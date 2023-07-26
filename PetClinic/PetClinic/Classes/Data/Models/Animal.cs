using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetClinic.Classes.Data.Models
{
    [Table("Animal")]
    public class Animal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Type { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Age must be a positive integer.")]
        public int Age { get; set; }

        [ForeignKey("Passport")]
        public string PassportSerialNumber { get; set; }

        [Required]
        public Passport Passport { get; set; }

        public List<Procedure> Procedures { get; set; }
    }
}
