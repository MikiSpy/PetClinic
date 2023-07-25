using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Classes.Data.Models
{
    [Table("Animal")]
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        public int Age { get; set; }

        public string PassportSerialNumber { get; set; }

        public Passport Passport { get; set; }
    }
}
