using PetClinic.Classes.Business;
using PetClinic.Classes.Data;
using PetClinic.Classes.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using PetClinic.Classes.Presentation;

internal class Program
{
    private static void Main(string[] args)
    {
        Display d = new Display();
        
    }
    public void ImportAnimalsFromJSON()
    {
        var dataImporter = new PetClinicController();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(folderPath, "Animals.json");
        dataImporter.ImportAnimalsFromJson(filePath);
    }
    public void ImportAnimalAidFromJSON()
    {
        var dataImporter = new PetClinicController();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(folderPath, "AnimalAid.json");

        List<AnimalAid> animalAidsToImport = dataImporter.DeserializeJsonAnimalAid(filePath);
        dataImporter.ImportDataJson(animalAidsToImport);
        if (animalAidsToImport.Count > 0)
        {
            string passportSerialNumber;
            foreach (AnimalAid animalAids in animalAidsToImport)
            {
                Console.WriteLine("Record" + animalAids.Name + "successfully imported.");
            }
        }
        else
        {
            Console.WriteLine("Error: JSON import unsuccessful. Invalid data.");
        }
    }
    public void ImportVetsFromXML()
    {
        var dataImporter = new PetClinicController();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(folderPath, "Vets.xml");
        List<Vet> vetsToImport = dataImporter.DeserializeXmlVets(filePath);

        if (vetsToImport.Count > 0)
        {
            foreach (Vet vets in vetsToImport)
            {
                Console.WriteLine("Record" + vets.Name + " successfully imported.");
            }
        }
        else
        {
            Console.WriteLine("Error: XML import unsuccessful. Invalid data.");
        }
    }
    public void ImportProceduresFromXML()
    {
        var dataImporter = new PetClinicController();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(folderPath, "Procedures.xml");
        List<Procedure> proceduresToImport = dataImporter.DeserializeXmlProcedure(filePath);

        if (proceduresToImport.Count > 0)
        {
            dataImporter.ImportProcedures(proceduresToImport);
        }
        else
        {
            Console.WriteLine("No Procedures found in the XML file.");
        }
    }
    public void ExportAnimalsToJSON()
    {
        var dataExporter = new PetClinicController();
        string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string jsonFilePath = Path.Combine(folderPath2, "Animals.json");

        dataExporter.ExportAnimalsToJson(jsonFilePath);

        Console.WriteLine("Animals exported to JSON successfully!");
    }
    public void ExportProceduresToXML()
    {
        using (var context = new PetClinicContext())
        {
            var proceduresForExport = context.Procedure
                .OrderBy(p => p.DateTime)
                .ThenBy(p => p.Animal.PassportSerialNumber)
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Procedure>));

            string folderPath3 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePathXML = Path.Combine(folderPath3, "Procedures.xml");

            using (FileStream fileStream = new FileStream(filePathXML, FileMode.Create))
            {
                serializer.Serialize(fileStream, proceduresForExport);
            }
        }
    }
}