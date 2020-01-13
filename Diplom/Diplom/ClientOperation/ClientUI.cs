using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class ClientUI//Консольный интерфейс клиента с выбором операции
    {
        public ClientUI(ClientAllData clientAllData)
        {
            ClientOperations clientOperations = new ClientOperations();

            while (true)
            {
                Console.WriteLine("\n1. Информация про мой счет\n");
                Console.WriteLine("2. Операции с балансом\n");
                Console.WriteLine("3. Кредитные операции\n");
                Console.WriteLine("4. Операции с депозитом\n");
                Console.WriteLine("5. Выйти из программы\n");

                Console.Write("Выберите номер операции - ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        clientOperations.ShowMyAccount(clientAllData);
                        break;
                    case "2":
                        clientOperations.BalanceOperation(clientAllData);
                        break;
                    case "3":
                        clientOperations.CreditOperation(clientAllData);
                        break;
                    case "4":
                        clientOperations.DepositOperation(clientAllData);
                        break;
                    case "5":
                        Console.WriteLine("\nВсего доброго! До свидания!\n");
                        return;
                    default:
                        Console.WriteLine("\nОперация не найдена\n");
                        break;
                }

                char proceedChar = 'д';
                Console.Write("Продолжить? (д/н) - ");
                string rerun = Console.ReadLine();
                Console.Clear();

                if(rerun == proceedChar.ToString())
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
