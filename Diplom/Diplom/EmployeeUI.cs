using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class EmployeeUI
    {
        public EmployeeUI(string employee)
        {
            EmployeeOperations employeeOperations = new EmployeeOperations();

            while (true)
            {
                Console.WriteLine($"Текущий сеанс открыл - {employee}");
                Console.WriteLine("\n1. Вывести информацию про клиентов\n");
                Console.WriteLine("2. Вывести расширенную информацию про клиентов\n");
                Console.WriteLine("3. Поиск среди клиентов\n");
                Console.WriteLine("4. Вывести закрепленных за менеджером клиентов\n");
                Console.WriteLine("5. Вывести всех сотрудников\n");
                Console.WriteLine("6. Добавить нового клиента\n");
                Console.WriteLine("7. Выйти из программы\n");

                Console.Write("Выберите номер операции - ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        employeeOperations.ShowShortInfoAboutClients();
                        break;
                    case "2":
                        employeeOperations.ShowExtendedInfoAboutClients();
                        break;
                    case "3":
                        employeeOperations.SearchInfoAboutClient();
                        break;
                    case "4":
                        employeeOperations.ShowManagerClients();
                        break;
                    case "5":
                        employeeOperations.ShowEmployeeInfo();
                        break;
                    case "6":
                        employeeOperations.NewClient();
                        break;
                    case "7":
                        Console.WriteLine("\nХорошего рабочего дня! До свидания!\n");
                        return;
                    default:
                        Console.WriteLine("\nОперация не найдена\n");
                        break;
                }

                Console.Write("Продолжить? (д/н) - ");
                string rerun = Console.ReadLine();
                Console.Clear();

                string next = "д";

                if(rerun == next)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
        }
    }
}