using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Classes.Data.Models
{
    [Table("Procedure")]
    public class Procedure
    {
        [Key]
        public int Id { get; set; }


        public int AnimalId { get; set; }

        public Animal Animal { get; set; }
        public int VetId { get; set; }

        public Vet Vet { get; set; }

        public List<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }

        public decimal Cost { get; set; }

        public DateTime DateTime { get; set; }

    }
}
