using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PetClinic.Classes.Data.Models
{
    [Table("AnimalAid")]
    public class AnimalAid
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public List<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }
    }
}
