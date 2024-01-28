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
                DisplayRecords(file); // для отображения записей из файла
                Console.ReadKey();
            }
            else if (choice == "2")
            {
                AddEmployeeRecord(file); // для добавления новой записи в файл
                DisplayRecords(file); // для отображения обновленных записей из файла
            }
            else
            {
                Console.WriteLine("Некорректный ввод.");
                Console.ReadLine();
                
            }
        }

        // для отображения записей из файла
        static void DisplayRecords(string file)
        {
            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    string[] elements = line.Split('#');
                    foreach (string element in elements)
                    {
                        Console.Write(element + "  ");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                // Если файла нет, выводим сообщение об ошибке
                Console.WriteLine("Файл не существует.");
                Console.ReadKey();
            }
        }

        // для добавления новой записи в файл
        static void AddEmployeeRecord(string file)
        {
            int id, age, height;
            string  timestamp, fullName, birthPlace, birthDate;

            Console.WriteLine("Введите данные сотрудника:");

            Console.Write("ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Введите корректный ID.");
                Console.Write("ID: ");
            }

            timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            Console.Write("Ф. И. О.: ");
            fullName = Console.ReadLine();

            Console.Write("Возраст: ");
            while (!int.TryParse(Console.ReadLine(), out age))
            {
                Console.WriteLine("Введите корректный возраст.");
                Console.Write("Возраст: ");
            } 

            Console.Write("Рост: ");
            while (!int.TryParse(Console.ReadLine(), out height))
            {
                Console.WriteLine("Введите корректный рост.");
                Console.Write("Рост: ");
            }

            Console.Write("Дата рождения (в формате ДД.ММ.ГГГГ): ");
            birthDate = Console.ReadLine();


            Console.Write("Место рождения: ");
            birthPlace = Console.ReadLine();

            // открываем файл для добавления записи
            string record = $"{id}#{timestamp}#{fullName}#{age}#{height}#{birthDate}#{birthPlace}";

            using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine(record);
            }

            Console.WriteLine("Запись добавлена в файл.");
            Console.ReadKey();
        }
    }
}
