using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class ClientAccess//Хранение данных о клиенте
    {
        public Dictionary<string, ClientAllData> data = new Dictionary<string, ClientAllData>();

        public ClientAccess()
        {
            ClientAllData allData = new ClientAllData();

            foreach (var item in allData.AddAllClientInfo())
            {
                data.Add(item.Phone, new ClientAllData(item.ClientID, item.Name, item.Surname, 
                    item.Birthday, item.Phone, item.InfoID, item.Balance, item.Credit, 
                    item.Deposit, item.Password, item.ManagerName, item.ManagerSurname, 
                    item.ManagerPhone));
            }
        }
    }
}