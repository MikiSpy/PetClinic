using Microsoft.VisualBasic;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClinic.Classes.Business;
using MySql.Data.MySqlClient;

namespace PetClinic.Classes.Presentation
{
    public class Display
    {
        // Конструктор на класа Display
        public Display()
        {
            Input();
        }

        // Метод за въвеждане на избор от потребителя
        public void Input()
        {
            var Program = new Program();

            // Извеждане на информация за наличните опции
            Info();

            int input = int.Parse(Console.ReadLine());

            // Обработка на избора на потребителя
            switch (input)
            {
                // Импорт на данни от JSON файл за животни
                case 1:
                    Program.ImportAnimalsFromJSON();
                    break;
                // Импорт на данни от JSON файл за помощи за животни
                case 2:
                    Program.ImportAnimalAidFromJSON();
                    break;
                // Импорт на процедури от XML файл
                case 3:
                    Program.ImportProceduresFromXML();
                    break;
                // Импорт на ветеринари от XML файл
                case 4:
                    Program.ImportVetsFromXML();
                    break;
                // Изход от програмата
                case 5:
                    Program.ExportProceduresToXML();
                    break;
                case 6:
                    Program.ExportAnimalsToJSON();
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                // Обработка на невалиден избор
                default:
                    Console.WriteLine("Invalid input:");
                    break;
            }
        }

        // Извеждане на информация за наличните опции
        public void Info()
        {
            Console.WriteLine("Welcome to the Pet Clinic!");
            Console.WriteLine("Available inputs:\n1 - Import Animals From JSON\n2 - Import Animal Aid From JSON\n3 - Import Procedures From XML\n4 - Import Vets From XML\n5 - Export Procedures To XML\n6 - Export Animals To JSON\n7 - Exit");
        }
    }
}