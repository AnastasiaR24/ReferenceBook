using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string file = "employee_records.txt";

            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - вывести данные на экран");
            Console.WriteLine("2 - добавить новую запись в файл");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                DisplayRecords(file);
            }
            else if (choice == "2")
            {
                AddEmployeeRecord(file);
                DisplayRecords(file);
            }
            else
            {
                Console.WriteLine("Некорректный ввод.");
                Console.ReadLine();
            }
        }

        static void DisplayRecords(string file)
        {
            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine("Файл не существует.");
                Console.ReadKey();
            }
        }

        static void AddEmployeeRecord(string file)
        {
            string id, timestamp, fullName, age, height, birthDate, birthPlace;

            Console.WriteLine("Введите данные сотрудника:");

            Console.Write("ID: ");
            id = Console.ReadLine();

            timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            Console.Write("Ф. И. О.: ");
            fullName = Console.ReadLine();

            Console.Write("Возраст: ");
            age = Console.ReadLine();

            Console.Write("Рост: ");
            height = Console.ReadLine();

            Console.Write("Дата рождения (в формате ДД.ММ.ГГГГ): ");
            birthDate = Console.ReadLine();

            Console.Write("Место рождения: ");
            birthPlace = Console.ReadLine();

            string record = $"{id}#{timestamp}#{fullName}#{age}#{height}#{birthDate}#{birthPlace}";

            using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine(record);
            }

            Console.WriteLine("Запись успешно добавлена в файл.");
            Console.ReadKey();
        }
    }
}
