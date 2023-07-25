namespace PetClinic.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Classes.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<PetClinic.Classes.Data.PetClinicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PetClinic.Classes.Data.PetClinicContext context)
        {

        }
    }
}
