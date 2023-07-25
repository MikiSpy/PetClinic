using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Classes.Data.Models
{
    [Table("ProcedureAnimalAid")]
    public class ProcedureAnimalAid
    {
        [Key]
        public int ProcedureId { get; set; }

        public int AnimalAidId { get; set; }

        public Procedure Procedure { get; set; }


        public AnimalAid AnimalAid { get; set; }
    }
}
