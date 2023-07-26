using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using PetClinic.Classes.Data;
using PetClinic.Classes.Data.Models;

namespace PetClinic.Classes.Business
{
    public class PetClinicController
    {
        public void ImportDataJson(List<AnimalAid> animalAids)
        {
            using (var context = new PetClinicContext())
            {
                foreach (var animalAid in animalAids)
                {
                    // Check if the price is positive
                    if (animalAid.Price <= 0)
                    {
                        Console.WriteLine($"Error: The price for '{animalAid.Name}' must be a positive number. Skipping import.");
                        continue;
                    }

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


        public List<AnimalAid> DeserializeJsonAnimalAid(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                List<AnimalAid> animalAids = JsonSerializer.Deserialize<List<AnimalAid>>(jsonContent);
                return animalAids;
            }
            catch (Exception ex)
            {
                // Показване на грешка при десериализация на JSON и връщане на празен списък
                Console.WriteLine($"Error while deserializing JSON: {ex.Message}");
                return new List<AnimalAid>();
            }
        }
        public void ImportAnimalsFromJson(string filePath)
        {
            List<Animal> animalsToImport = DeserializeJsonAnimals(filePath);

            if (animalsToImport.Count > 0)
            {
                using (var context = new PetClinicContext())
                {
                    var existingAnimalSerialNumbers = context.Animal
                        .Where(a => a.Passport != null)
                        .Select(a => a.Passport.SerialNumber)
                        .ToList();

                    animalsToImport = animalsToImport
                        .Where(a => a.Passport != null && !existingAnimalSerialNumbers.Contains(a.Passport.SerialNumber))
                        .ToList();

                    if (animalsToImport.Count > 0)
                    {
                        context.Animal.AddRange(animalsToImport);
                        context.SaveChanges();
                        Console.WriteLine($"Successfully imported {animalsToImport.Count} animals.");
                    }
                    else
                    {
                        Console.WriteLine("No new animals to import.");
                    }
                }
            }
            else
            {
                Console.WriteLine("No animals found in the JSON file.");
            }
        }


        private List<Animal> DeserializeJsonAnimals(string filePath)
        {
            // Прочитане на съдържанието на JSON файл и десериализация към списък с Animal обекти
            string jsonContent = File.ReadAllText(filePath);
            List<Animal> animals = JsonSerializer.Deserialize<List<Animal>>(jsonContent);
            return animals;
        }

        public void ExportAnimalsToJson(string filePath)
        {
            using (var context = new PetClinicContext())
            {
                // Зареждане на всички животни от базата данни с включени Passport обекти
                // Сортиране по телефонен номер на собственика и възраст
                var animals = context.Animal
                    .Include(a => a.Passport)
                    .OrderBy(a => a.Passport.OwnerPhoneNumber)
                    .ThenBy(a => a.Age)
                    .ToList();

                // Създаване на анонимен обект за съхранение на необходимите данни
                var animalData = animals.Select(a => new
                {
                    OwnerName = a.Passport?.OwnerName,
                    AnimalName = a.Name,
                    Age = a.Age,
                    SerialNumber = a.Passport?.SerialNumber,
                    RegisteredOn = a.Passport?.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                }).ToList();

                // Сериализация на данните в JSON формат с отстъпи и специално кодиране
                string jsonContent = JsonSerializer.Serialize(animalData, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                // Записване на JSON съдържанието в текстов файл
                File.WriteAllText(filePath, jsonContent);
            }
        }

        public List<Vet> DeserializeXmlVets(string filePath)
        {
            string xmlContent = File.ReadAllText(filePath);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Vet>), new XmlRootAttribute("Vets"));

            using (StringReader stringReader = new StringReader(xmlContent))
            {
                List<Vet> vets = (List<Vet>)serializer.Deserialize(stringReader);
                return vets;
            }
        }


        public List<Procedure> DeserializeXmlProcedure(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Procedure>), new XmlRootAttribute("Procedures"));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                
                List<Procedure> procedures = (List<Procedure>)serializer.Deserialize(fileStream);
                return procedures;
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
                using (var context = new PetClinicContext())
                {
                    foreach (Vet vet in vetsToImport)
                    {
                        // Check if the vet already exists in the database
                        if (context.Vet.Any(v => v.Name == vet.Name))
                        {
                            Console.WriteLine($"Error: The vet '{vet.Name}' already exists. Skipping import.");
                            continue;
                        }

                        // Import the vet if it does not already exist
                        context.Vet.Add(vet);
                        context.SaveChanges();
                        Console.WriteLine($"Record {vet.Name} successfully imported.");
                    }
                }
            }
            else
            {
                Console.WriteLine("No vets found in the XML file.");
            }
        }

        public void ImportProcedures(List<Procedure> proceduresToImport)
        {
            using (var context = new PetClinicContext())
            {
                foreach (Procedure procedure in proceduresToImport)
                {
                    if (procedure.Vet == null)
                    {
                        Console.WriteLine("Error: Invalid data. Vet information is missing.");
                        continue;
                    }

                    Vet vet = context.Vet.FirstOrDefault(v => v.Name == procedure.Vet.Name);
                    if (vet == null)
                    {
                        Console.WriteLine($"Error: Invalid data. Vet '{procedure.Vet.Name}' does not exist.");
                        continue;
                    }

                    if (procedure.Animal == null || procedure.Animal.Passport == null)
                    {
                        Console.WriteLine("Error: Invalid data. Animal or passport information is missing.");
                        continue;
                    }

                    Animal animal = context.Animal.FirstOrDefault(a => a.Passport.SerialNumber == procedure.Animal.Passport.SerialNumber);
                    if (animal == null)
                    {
                        Console.WriteLine($"Error: Invalid data. Animal with serial number '{procedure.Animal.Passport.SerialNumber}' does not exist.");
                        continue;
                    }

                    var animalAids = procedure.ProcedureAnimalAids?.Select(a => a.AnimalAid).ToList();
                    if (animalAids == null || animalAids.Count == 0)
                    {
                        Console.WriteLine("Error: Invalid data. Animal aids information is missing.");
                        continue;
                    }

                    context.Procedure.Add(procedure);
                    context.SaveChanges();

                    Console.WriteLine($"Record successfully imported.");
                }
            }
        }

    }
}