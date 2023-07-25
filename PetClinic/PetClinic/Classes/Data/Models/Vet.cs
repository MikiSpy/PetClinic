using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace PetClinic.Classes.Data.Models
{
    [Table("Vet")]
    public class Vet
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Profession { get; set; }

        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public List<Procedure> Procedures { get; set; }
    }
}
