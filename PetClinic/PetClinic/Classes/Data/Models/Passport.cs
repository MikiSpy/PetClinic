using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Classes.Data.Models
{
    [Table("Passport")]
    public class Passport
    {
        [Key]
        public string SerialNumber { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OwnerName { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Animal Animal { get; set; }
    }
}
