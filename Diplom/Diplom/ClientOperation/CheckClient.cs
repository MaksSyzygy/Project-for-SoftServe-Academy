using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class CheckClient//Проверка логина и пароля клиента
    {
        public CheckClient()
        {
            ClientAccess key = new ClientAccess();
            ClientAllData clientAllData = new ClientAllData();

            while (true)
            {
                Console.Write("Номер телефона - ");//0503127845
                string inputPhone = Console.ReadLine();
                string formatPhone = string.Format("{0:(0##)###-##-##}", Convert.ToInt64(inputPhone));

                if(key.data.TryGetValue(formatPhone, out clientAllData) == false)
                {
                    Console.WriteLine($"Клиент по номеру {formatPhone} не найден");
                    continue;
                }

                while (true)
                {
                    Console.Write($"Введите пароль для аккаунта {formatPhone} - ");//12345
                    string password = Console.ReadLine();

                    if (password != clientAllData.Password)
                    {
                        Console.WriteLine("Неправильный пароль");
                        continue;
                    }
                    break;
                }

                Console.Clear();
                ClientUI clientUI = new ClientUI(clientAllData);
                break;
            }
        }
    }
}