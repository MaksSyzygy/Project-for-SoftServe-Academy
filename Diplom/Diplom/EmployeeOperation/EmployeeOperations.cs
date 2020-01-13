using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class EmployeeOperations//Поиск и вывод информации о клиентах, сотрудниках, закрепленных за менеджером клиентов
                           //и создание нового клиента
    {
        DBOperations operations = new DBOperations();
        DBClientConfidentialFields clientConfidentialFields = new DBClientConfidentialFields();
        DBEmployeeConfidentialFields employeeConfidentialFields = new DBEmployeeConfidentialFields();
        CreateClient createClient = new CreateClient();

        public void ShowShortInfoAboutClients()
        {
            foreach(var item in operations.GetClientFields())
            {
                Console.WriteLine($"\n{item.Intro()}\n");
                Console.WriteLine(new string('-', 50));
            }
        }

        public void ShowExtendedInfoAboutClients()
        {
            var allInfo = from info in operations.GetClientFields()
                          join confInfo in clientConfidentialFields.GetClientConfidentialFields()
                          on info.ID equals confInfo.ID
                          join manager in operations.GetEmployeeFields()
                          on info.Manager equals manager.ID
                          select new
                          {
                              ClientID = info.ID,
                              ClientName = info.Name,
                              ClientSurname = info.Surname,
                              ClientBirthday = info.Birthday,
                              ClientPhone = info.Phone,
                              Balance = confInfo.Balance,
                              Credit = confInfo.Credit,
                              Deposit = confInfo.Deposit,
                              ManagerName = manager.Name,
                              ManagerSurname = manager.Surname
                          };

            foreach (var item in allInfo)
            {
                Console.WriteLine($"\nId - {item.ClientID}, Имя - {item.ClientName}, Фамилия - {item.ClientSurname}\n" +
                    $"Дата Рождения - {item.ClientBirthday}, Телефон - {item.ClientPhone}\n" +
                    $"Баланс - {item.Balance}, Кредит - {item.Credit}, Депозит - {item.Deposit}" +
                    $"\nМенеджер - {item.ManagerName} {item.ManagerSurname}\n");
                Console.WriteLine(new string('-', 50));
            }
        }

        public void SearchInfoAboutClient()
        {
            Console.Write("Поисковый запрос - ");
            string query = Console.ReadLine();

            List<DBOperations> list = new List<DBOperations>();
            list.AddRange(operations.GetClientFields());

            var filter = list.Where(x => x.Intro().Contains(query));

            if(filter.Count() == 0)
            {
                Console.WriteLine($"Запрос \"{query}\" не найден");
            }
            else
            {
                foreach (var item in filter)
                {
                    Console.WriteLine($"\n{item.Intro()}");
                    Console.Write($"\n{new string('-', 50)}\n");
                }
            }
        }

        public void ShowManagerClients()
        {
            var groupClients = from empl in operations.GetEmployeeFields()
                               join client in operations.GetClientFields()
                               on empl.ID equals client.Manager into grouping
                               orderby grouping.Count() descending
                               select new
                               {
                                   Manager = $"Менеджер - {empl.Name} {empl.Surname}",
                                   ClientCount = grouping.Count(),
                                   Client = grouping
                               };

            foreach(var manager in groupClients)
            {
                Console.WriteLine($"\n{manager.Manager}, Кол-во клиентов - {manager.ClientCount}\n");
                foreach(var client in manager.Client)
                {
                    Console.WriteLine($"\t\tКлиент - {client.Name} {client.Surname}\n\t\tТелефон - {client.Phone}\n");
                }
                Console.WriteLine(new string('-', 50));
            }
        }

        public void ShowEmployeeInfo()
        {
            var managers = from manager in operations.GetEmployeeFields()
                           join details in employeeConfidentialFields.GetEmployeeConfidentialFields()
                           on manager.ID equals details.ID
                           select new
                           {
                               ID = manager.ID,
                               Name = manager.Name,
                               Surname = manager.Surname,
                               Salary = details.Salary,
                               Position = details.Position
                           };

            foreach(var item in managers)
            {
                Console.WriteLine($"\n{item.ID}. {item.Name} {item.Surname}\nЗарплата - {item.Salary}, " +
                    $"Должность - {item.Position}\n");
                Console.WriteLine(new string('-', 50));
            }
        }

        public void NewClient()
        {
            createClient.AddNewCLient();
        }
    }
}