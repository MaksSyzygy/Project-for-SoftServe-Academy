using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class OperationInterface//Выбор варианта работы приложения
    {
        public OperationInterface()
        {
            EmployeeOperations employeeOperations = new EmployeeOperations();
            
            Console.Write("Выберите аккаунт (Сотрудник/Клиент) - ");
            string input = Console.ReadLine();

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string formatInput = textInfo.ToTitleCase(input);

            switch (formatInput)
            {
                case "Сотрудник":
                    CheckEmployee checkEmployee = new CheckEmployee();
                    break;
                case "Клиент":
                    CheckClient checkClient = new CheckClient();
                    break;
                default:
                    Console.WriteLine("Повторите ввод");
                    break;
            }
        }
    }
}
