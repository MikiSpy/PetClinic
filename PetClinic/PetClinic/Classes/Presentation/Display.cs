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

        public Display()
        {
            Input();
        }

        public void Input()
        {
            var Program = new Program();

            Info();

            int input = int.Parse(Console.ReadLine());
            
            switch (input)
            {
                
                case 1:
                    Program.ImportAnimalsFromJSON();
                    break;
                case 2:
                    Program.ImportAnimalAidFromJSON();
                    break;
                case 3:
                    Program.ImportProceduresFromXML();
                    break;
                case 4:
                    Program.ImportVetsFromXML();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input:");
                    break;
            }
        }
        public void Info()
        {
            Console.WriteLine("Welcome to the Pet Clinic!");
            Console.WriteLine("Available inputs:\n1 - Import Animals From JSON\n2 - Import Animal Aid From JSON\n3 - Import Procedures From XML\n4 - Import Vets From XML\n5 - Exit");
        }
    }
}
