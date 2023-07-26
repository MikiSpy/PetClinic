using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Classes.Data.Models
{
    [Table("Procedure")]
    public class Procedure
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AnimalId { get; set; }

        [ForeignKey("AnimalId")]
        public Animal Animal { get; set; }

        [Required]
        public int VetId { get; set; }

        [ForeignKey("VetId")]
        public Vet Vet { get; set; }

        public List<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }

        public decimal Cost { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}
