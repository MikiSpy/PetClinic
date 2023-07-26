namespace PetClinic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProcedureAnimalAid", "Procedure_Id", "dbo.Procedure");
            DropIndex("dbo.ProcedureAnimalAid", new[] { "Procedure_Id" });
            DropColumn("dbo.ProcedureAnimalAid", "ProcedureId");
            RenameColumn(table: "dbo.ProcedureAnimalAid", name: "Procedure_Id", newName: "ProcedureId");
            DropPrimaryKey("dbo.ProcedureAnimalAid");
            AlterColumn("dbo.Animal", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Animal", "Type", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Passport", "OwnerPhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Passport", "OwnerName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.AnimalAid", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.ProcedureAnimalAid", "ProcedureId", c => c.Int(nullable: false));
            AlterColumn("dbo.ProcedureAnimalAid", "ProcedureId", c => c.Int(nullable: false));
            AlterColumn("dbo.Vet", "Name", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Vet", "Profession", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Vet", "PhoneNumber", c => c.String(nullable: false));
            AddPrimaryKey("dbo.ProcedureAnimalAid", new[] { "ProcedureId", "AnimalAidId" });
            CreateIndex("dbo.ProcedureAnimalAid", "ProcedureId");
            CreateIndex("dbo.AnimalAid", "Name", unique: true);
            AddForeignKey("dbo.ProcedureAnimalAid", "ProcedureId", "dbo.Procedure", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProcedureAnimalAid", "ProcedureId", "dbo.Procedure");
            DropIndex("dbo.AnimalAid", new[] { "Name" });
            DropIndex("dbo.ProcedureAnimalAid", new[] { "ProcedureId" });
            DropPrimaryKey("dbo.ProcedureAnimalAid");
            AlterColumn("dbo.Vet", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Vet", "Profession", c => c.String());
            AlterColumn("dbo.Vet", "Name", c => c.String());
            AlterColumn("dbo.ProcedureAnimalAid", "ProcedureId", c => c.Int());
            AlterColumn("dbo.ProcedureAnimalAid", "ProcedureId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AnimalAid", "Name", c => c.String());
            AlterColumn("dbo.Passport", "OwnerName", c => c.String());
            AlterColumn("dbo.Passport", "OwnerPhoneNumber", c => c.String());
            AlterColumn("dbo.Animal", "Type", c => c.String());
            AlterColumn("dbo.Animal", "Name", c => c.String());
            AddPrimaryKey("dbo.ProcedureAnimalAid", "ProcedureId");
            RenameColumn(table: "dbo.ProcedureAnimalAid", name: "ProcedureId", newName: "Procedure_Id");
            AddColumn("dbo.ProcedureAnimalAid", "ProcedureId", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.ProcedureAnimalAid", "Procedure_Id");
            AddForeignKey("dbo.ProcedureAnimalAid", "Procedure_Id", "dbo.Procedure", "Id");
        }
    }
}
