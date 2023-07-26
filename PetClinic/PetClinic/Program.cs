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
        // Инициализиране на обект от класа Display, който ще представлява интерфейса на програмата
        Display d = new Display();
    }

    // Метод за импорт на данни от JSON файл за животни
    public void ImportAnimalsFromJSON()
    {
        // Създаване на инстанция на PetClinicController за обработка на данните
        var dataImporter = new PetClinicController();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(folderPath, "Animals.json");

        // Импорт на данните
        dataImporter.ImportAnimalsFromJson(filePath);
    }

    // Метод за импорт на данни от JSON файл за помощи за животни
    public void ImportAnimalAidFromJSON()
    {
        // Създаване на инстанция на PetClinicController за обработка на данните
        var dataImporter = new PetClinicController();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(folderPath, "AnimalAid.json");

        // Импорт на помощи за животни от JSON файл
        List<AnimalAid> animalAidsToImport = dataImporter.DeserializeJsonAnimalAid(filePath);

        // Извеждане на резултат от импорта
        if (animalAidsToImport.Count > 0)
        {
            using (var context = new PetClinicContext())
            {
                foreach (AnimalAid animalAid in animalAidsToImport)
                {
                    // Проверка дали услугата вече съществува в базата данни
                    if (context.AnimalAid.Any(a => a.Name == animalAid.Name))
                    {
                        Console.WriteLine($"Error: The service '{animalAid.Name}' already exists. Skipping import.");
                        continue;
                    }

                    // Валидиране на обекта "animalAid" преди да се импортира
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(animalAid);

                    if (!Validator.TryValidateObject(animalAid, validationContext, validationResults, true))
                    {
                        // Показване на грешки при невалидни данни и пропускане на вноса
                        foreach (var result in validationResults)
                        {
                            Console.WriteLine($"Error: {result.ErrorMessage}. Skipping import.");
                        }
                        continue;
                    }

                    // Импортиране на услугата, ако премине всички проверки за валидност
                    context.AnimalAid.Add(animalAid);
                    context.SaveChanges();
                    Console.WriteLine($"Record {animalAid.Name} successfully imported.");
                }
            }
        }
        else
        {
            Console.WriteLine("Error: JSON import unsuccessful. Invalid data.");
        }
    }


    // Метод за импорт на данни за ветеринари от XML файл
    public void ImportVetsFromXML()
    {
        // Създаване на инстанция на PetClinicController за обработка на данните
        var dataImporter = new PetClinicController();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(folderPath, "Vets.xml");

        // Импорт на данните за ветеринарите от XML файл
        List<Vet> vetsToImport = dataImporter.DeserializeXmlVets(filePath);

        // Извеждане на резултат от импорта
        if (vetsToImport.Count > 0)
        {
            foreach (Vet vets in vetsToImport)
            {
                Console.WriteLine("Record " + vets.Name + " successfully imported.");
            }
        }
        else
        {
            Console.WriteLine("Error: XML import unsuccessful. Invalid data.");
        }
    }

    // Метод за импорт на процедури от XML файл
    public void ImportProceduresFromXML()
    {
        // Създаване на инстанция на PetClinicController за обработка на данните
        var dataImporter = new PetClinicController();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(folderPath, "Procedures.xml");

        // Импорт на процедури от XML файл
        List<Procedure> proceduresToImport = dataImporter.DeserializeXmlProcedure(filePath);

        // Извеждане на резултат от импорта
        if (proceduresToImport.Count > 0)
        {
            dataImporter.ImportProcedures(proceduresToImport);
        }
        else
        {
            Console.WriteLine("No Procedures found in the XML file.");
        }
    }

    // Метод за експорт на данни за животните в JSON формат
    public void ExportAnimalsToJSON()
    {
        // Създаване на инстанция на PetClinicController за обработка на данните
        var dataExporter = new PetClinicController();
        string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string jsonFilePath = Path.Combine(folderPath2, "AnimalsExport.json");

        // Експорт на данните за животните в JSON формат
        dataExporter.ExportAnimalsToJson(jsonFilePath);

        Console.WriteLine("Animals exported to JSON successfully!");
    }

    // Метод за експорт на процедури в XML формат
    public void ExportProceduresToXML()
    {
        // Създаване на инстанция на PetClinicContext за достъп до базата данни
        using (var context = new PetClinicContext())
        {
            // Извличане на процедурите от базата данни и сортиране по дата и сериен номер на животно
            var proceduresForExport = context.Procedure
                .OrderBy(p => p.DateTime)
                .ThenBy(p => p.Animal.PassportSerialNumber)
                .ToList();

            // Създаване на сериализатор за XML
            XmlSerializer serializer = new XmlSerializer(typeof(List<Procedure>));

            string folderPath3 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePathXML = Path.Combine(folderPath3, "ProceduresExport.xml");

            // Експорт на процедурите в XML файл
            using (FileStream fileStream = new FileStream(filePathXML, FileMode.Create))
            {
                serializer.Serialize(fileStream, proceduresForExport);
            }
        }
        Console.WriteLine("Data exported to XML successfully!");
    }
}
