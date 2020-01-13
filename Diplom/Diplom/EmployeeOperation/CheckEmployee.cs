using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class CheckEmployee//Проверка фамилии сотрудника
    {
        public CheckEmployee()
        {
            DBOperations operations = new DBOperations();

            while (true)
            {
                Console.Write("Введите ID сотрудника - ");//Иванов
                string surname = Console.ReadLine();

                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                string formatSurname = textInfo.ToTitleCase(surname);

                var query = GetEmplSurname().Find(x => x == formatSurname);
                var pullName = from item in operations.GetEmployeeFields()
                                   where item.Surname == query
                                   select item;

                string fullName = null;

                foreach(var item in pullName)
                {
                    fullName = $"{item.Name} {item.Surname}";
                }

                if (query != formatSurname)
                {
                    Console.WriteLine("Не найдено");
                    continue;
                }
                else
                {
                    Console.Clear();
                    EmployeeUI employeeUI = new EmployeeUI(fullName);
                }
                break;
            }
        }

        private List<string> GetEmplSurname()
        {
            DBOperations operations = new DBOperations();
            List<string> surnames = new List<string>();
            foreach (var item in operations.GetEmployeeFields())
            {
                surnames.Add(item.Surname);
            }

            return surnames;
        }
    }
}