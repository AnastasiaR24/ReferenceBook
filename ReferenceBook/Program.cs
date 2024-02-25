using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBook
{
    internal class Program
    {
        struct Worker
        {
            public int Id { get; set; }
            public DateTime DateAdded { get; set; }
            public string FullName { get; set; }
            public int Age { get; set; }
            public int Height { get; set; }
            public DateTime BirthDate { get; set; }
            public string BirthPlace { get; set; }
        }

        class Repository
        {
            private string file = "employee_records.txt";

           
           //  Просмотр всех записей
            public Worker[] GetAllWorkers()
            {
                if (File.Exists(file))
                {
                    string[] lines = File.ReadAllLines(file);
                    List<Worker> workers = new List<Worker>();

                    foreach (string line in lines)
                    {
                        string[] elements = line.Split('#');
                        Worker worker = new Worker
                        {
                            Id = int.Parse(elements[0]),
                            DateAdded = DateTime.Parse(elements[1]),
                            FullName = elements[2],
                            Age = int.Parse(elements[3]),
                            Height = int.Parse(elements[4]),
                            BirthDate = DateTime.Parse(elements[5]),
                            BirthPlace = elements[6]
                        };
                        workers.Add(worker);
                    }

                    return workers.ToArray();
                }
                else
                {
                    Console.WriteLine("Файл не существует.");
                    return new Worker[0];
                }
            }

            // Создание записи
            public void AddWorker(Worker worker)
            {
                if (!File.Exists(file))
                {
                    File.Create(file).Close();
                }

                string[] existingRecords = File.ReadAllLines(file);
                if (existingRecords.Length == 0)
                {
                    worker.Id = 1;
                }
                else
                {
                    worker.Id = existingRecords.Length + 1;
                }
                worker.DateAdded = DateTime.Now;

                string record = $"{worker.Id}#{worker.DateAdded.ToString("dd.MM.yyyy HH:mm")}#{worker.FullName}#{worker.Age}#{worker.Height}#{worker.BirthDate.ToString("dd.MM.yyyy")}#{worker.BirthPlace}";

                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(record);
                }

                Console.WriteLine("Запись добавлена в файл.");
            }

            // Удаление записи
            public void DeleteWorker(int id)
            {
                Worker[] workers = GetAllWorkers().Where(worker => worker.Id != id).ToArray();
                WriteWorkersToFile(workers);
            }

            // Вывод записей в выбранном диапазоне дат
            public List<Worker> LoadRecordsInDateRange(DateTime startDate, DateTime endDate)
            {
                List<Worker> workers = GetAllWorkers().Where(w => w.DateAdded >= startDate && w.DateAdded <= endDate).ToList();

                if (workers.Count == 0)
                {
                    Console.WriteLine("Нет записей о сотрудниках в заданном диапазоне дат.");
                }

                return workers;
            }

            // вывод записи по ID
            public Worker GetWorkerById(int id)
            {
                string[] lines = File.ReadAllLines(file);

                foreach (string line in lines)
                {
                    string[] elements = line.Split('#');
                    if (int.Parse(elements[0]) == id)
                    {
                        Worker worker = new Worker
                        {
                            Id = int.Parse(elements[0]),
                            DateAdded = DateTime.Parse(elements[1]),
                            FullName = elements[2],
                            Age = int.Parse(elements[3]),
                            Height = int.Parse(elements[4]),
                            BirthDate = DateTime.Parse(elements[5]),
                            BirthPlace = elements[6]
                        };

                        return worker;
                    }
                }

                return new Worker(); 
            }

            // приватная запись данных
            private void WriteWorkersToFile(Worker[] workers)
            {
                string[] lines = workers.Select(worker => $"{worker.Id}#{worker.DateAdded}#{worker.FullName}#{worker.Age}#{worker.Height}#{worker.BirthDate}#{worker.BirthPlace}").ToArray();
                File.WriteAllLines(file, lines);
            }
        }

        static void Main(string[] args)
        {
            Repository repository = new Repository();
            string file = "employee_records.txt";
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1 - отобразить данные на экран");
                Console.WriteLine("2 - добавить новую запись");
                Console.WriteLine("3 - удалить запись");
                Console.WriteLine("4 - загрузить записи в диапазоне дат");
                Console.WriteLine("5 - найти запись по ID");
                Console.WriteLine("6 - выйти");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayRecords(file); // для отображения записей из файла
                        break;
                    case "2":
                        AddEmployeeRecord(file); // для добавления новой записи в файл
                        break;
                    case "3":
                        DeleteEmployeeRecord(repository); // для удаления записи 
                        break;
                    case "4":
                        LoadRecordsInDateRange(repository); // для загрузки записей в диапазоне дат
                        break;
                    case "5":
                        GetWorkerByIdy(repository); // для отображения одной записи из файла по ID
                        break;
                    case "6":
                        Environment.Exit(0); // выход
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод.");
                        break;
                }
            }
            Console.ReadLine();
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
                // если файла нет, выводим сообщение об ошибке
                Console.WriteLine("Файл не существует.");
                Console.ReadKey();
            }
        }

        // для добавления новой записи в файл
        static void AddEmployeeRecord(string file)
        {
            int id, age, height;
            string timestamp, fullName, birthPlace, birthDate;

            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }

            string[] existingRecords = File.ReadAllLines(file);
            if (existingRecords.Length == 0)
            {
                id = 1;
            }
            else
            {
                id = existingRecords.Length + 1;
            }

            Console.WriteLine("Введите данные сотрудника:");

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

        // удаление записи
        static void DeleteEmployeeRecord(Repository repository)
        {
            Console.Write("Введите ID записи для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                repository.DeleteWorker(id);
                Console.WriteLine("Запись успешно удалена!");
            }
            else
            {
                Console.WriteLine("Некорректный ввод ID.");
            }
            Console.ReadKey();

        }

        // Вывод записей в выбранном диапазоне дат
        static void LoadRecordsInDateRange(Repository repository)
        {
            Console.Write("Введите начальную дату (в формате ДД.ММ.ГГГГ): ");
            DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

            Console.Write("Введите конечную дату (в формате ДД.ММ.ГГГГ): ");
            DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

            List<Worker> workersInRange = repository.LoadRecordsInDateRange(startDate, endDate);

            if (workersInRange.Count == 0)
            {
                Console.WriteLine("Нет записей о сотрудниках в указанном диапазоне дат.");
            }
            else
            {
                Console.WriteLine("Найденные записи:");
                foreach (var worker in workersInRange)
                {
                    Console.WriteLine($"{worker.Id} {worker.DateAdded} {worker.FullName} {worker.Age} {worker.Height} {worker.BirthDate} {worker.BirthPlace}");

                }
            }
        }

        // вывод записи по ID
        static void GetWorkerByIdy(Repository repository)
        {
            Console.Write("Введите ID записи для поиска: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Worker foundWorker = repository.GetWorkerById(id);
                if (foundWorker.Equals(default(Worker)))
                {
                    Console.WriteLine("Запись с указанным ID не найдена.");
                }
                else
                {
                    Console.WriteLine($"Найденная запись:");
                    Console.WriteLine($"{foundWorker.Id} {foundWorker.DateAdded} {foundWorker.FullName} {foundWorker.Age} {foundWorker.Height} {foundWorker.BirthDate}");
                   
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод ID.");
            }
            Console.ReadKey();
        }

    }
}
