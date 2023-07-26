using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Classes.Data.Models
{
    [Table("ProcedureAnimalAid")]
    public class ProcedureAnimalAid
    {
        [Key]
        [Column(Order = 1)]
        public int ProcedureId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int AnimalAidId { get; set; }

        [ForeignKey("ProcedureId")]
        public Procedure Procedure { get; set; }

        [ForeignKey("AnimalAidId")]
        public AnimalAid AnimalAid { get; set; }
    }
}
