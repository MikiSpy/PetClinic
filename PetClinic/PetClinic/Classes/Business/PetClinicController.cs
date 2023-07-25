using System;
using System.Collections.Generic;
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
        // JSON import and export methods
        public void ImportDataJson(List<AnimalAid> animalAids)
        {
            using (var context = new PetClinicContext())
            {
                context.AnimalAid.AddRange(animalAids);
                context.SaveChanges();
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
                Console.WriteLine($"Error while deserializing JSON: {ex.Message}");
                return new List<AnimalAid>(); // Return an empty list to indicate no data was successfully deserialized.
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
            string jsonContent = File.ReadAllText(filePath);
            List<Animal> animals = JsonSerializer.Deserialize<List<Animal>>(jsonContent);
            return animals;
        }

        public void ExportAnimalsToJson(string filePath)
        {
            using (var context = new PetClinicContext())
            {
                var animals = context.Animal
                    .Include(a => a.Passport)
                    .OrderBy(a => a.Passport.OwnerPhoneNumber)
                    .ThenBy(a => a.Age)
                    .ToList();

                var animalData = animals.Select(a => new
                {
                    OwnerName = a.Passport?.OwnerName,
                    AnimalName = a.Name,
                    Age = a.Age,
                    SerialNumber = a.Passport?.SerialNumber,
                    RegisteredOn = a.Passport?.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                }).ToList();

                string jsonContent = JsonSerializer.Serialize(animalData, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                File.WriteAllText(filePath, jsonContent);
            }
        }

        // XML import and export methods for Vets
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

        // XML import and import methods for Procedures
        public List<Procedure> DeserializeXmlProcedure(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Procedure>));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                List<Procedure> procedures = (List<Procedure>)serializer.Deserialize(fileStream);
                return procedures;
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