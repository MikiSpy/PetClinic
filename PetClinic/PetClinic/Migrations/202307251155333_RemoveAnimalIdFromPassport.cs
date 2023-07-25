namespace PetClinic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAnimalIdFromPassport : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Passport", "AnimalId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Passport", "AnimalId", c => c.Int(nullable: false));
        }
    }
}
