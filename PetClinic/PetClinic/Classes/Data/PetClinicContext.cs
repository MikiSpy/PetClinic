using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using PetClinic.Classes.Data.Models;

namespace PetClinic.Classes.Data
{
    public class PetClinicContext : DbContext
    {
        // Конструктор на контекста
        public PetClinicContext() : base("Data Source=(localdb)\\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False") { }

        // Дефиниране на DbSet за всяка моделна класа
        public DbSet<Animal> Animal { get; set; }
        public DbSet<AnimalAid> AnimalAid { get; set; }
        public DbSet<Passport> Passport { get; set; }
        public DbSet<Procedure> Procedure { get; set; }
        public DbSet<ProcedureAnimalAid> ProcedureAnimalAid { get; set; }
        public DbSet<Vet> Vet { get; set; }

        // Презаписване на метода OnModelCreating, за да дефинираме връзката между Animal и Passport
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasOptional(a => a.Passport)
                .WithRequired(p => p.Animal);
            modelBuilder.Entity<ProcedureAnimalAid>()

        .HasKey(pa => new { pa.ProcedureId, pa.AnimalAidId }); // Define composite primary key

            modelBuilder.Entity<ProcedureAnimalAid>()
                .HasRequired(pa => pa.Procedure) // Configure the foreign key to Procedure entity
                .WithMany(p => p.ProcedureAnimalAids)
                .HasForeignKey(pa => pa.ProcedureId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ProcedureAnimalAid>()
                .HasRequired(pa => pa.AnimalAid) // Configure the foreign key to AnimalAid entity
                .WithMany(a => a.ProcedureAnimalAids)
                .HasForeignKey(pa => pa.AnimalAidId)
                .WillCascadeOnDelete(true);
        }
    }
}

