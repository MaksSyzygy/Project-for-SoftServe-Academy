using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class ClientAllData//Данные о клиенте из родительской, дочерней и смежной таблице
    {
        #region PrivateFields
        private int clientID;
        private string name;
        private string surname;
        private string birthday;
        private string phone;
        private int infoID;
        private decimal balance;
        private decimal credit;
        private decimal deposit;
        private string password;
        private string managerName;
        private string managerSurname;
        private string managerPhone;
        #endregion
        #region fieldProperties
        public int ClientID { get { return clientID; } }

        public string Name { get { return name; } }

        public string Surname { get { return surname; } }

        public string Birthday { get { return birthday; } }

        public string Phone { get { return phone; } }

        public int InfoID { get { return infoID; } }

        public decimal Balance { get { return balance; } set { balance = value; } }

        public decimal Credit { get { return credit; } set { credit = value; } }

        public decimal Deposit { get { return deposit; } set { deposit = value; } }

        public string Password { get { return password; } }

        public string ManagerName { get { return managerName; } }

        public string ManagerSurname { get { return managerSurname; } }

        public string ManagerPhone { get { return managerPhone; } }
        #endregion

        public string Intro()
        {
            return $"Клиент - {Name} {Surname}\n\t\tДата Рождения - {Birthday}\n\t\tТелефон - {Phone}" +
                $"\n\t\tБаланс - {Balance}\n\t\tКредит - {Credit}\n\t\tДепозит - {Deposit}\n\t\t" +
                $"Мой менеджер - {ManagerName} {ManagerSurname}, {ManagerPhone}";
        }

        public ClientAllData(int clientID, string name, string surname, string birthday, string phone, 
            int infoID, decimal balance, decimal credit, decimal deposit, string password, 
            string managerName, string managerSurname, string managerPhone)
        {
            this.clientID = clientID;
            this.name = name;
            this.surname = surname;
            this.birthday = birthday;
            this.phone = phone;
            this.infoID = infoID;
            this.balance = balance;
            this.credit = credit;
            this.deposit = deposit;
            this.password = password;
            this.managerName = managerName;
            this.managerSurname = managerSurname;
            this.managerPhone = managerPhone;
        }

        public ClientAllData() { }

        public List<ClientAllData> AddAllClientInfo()
        {
            DBOperations operations = new DBOperations();
            DBClientConfidentialFields clientConfidentialFields = new DBClientConfidentialFields();
            DBEmployeeConfidentialFields employeeConfidentialFields = new DBEmployeeConfidentialFields();

            var allInfo = from mainFields in operations.GetClientFields()
                          join confFields in clientConfidentialFields.GetClientConfidentialFields()
                          on mainFields.ID equals confFields.ID

                          join myManager in operations.GetEmployeeFields()
                          on mainFields.Manager equals myManager.ID

                          join managerInfo in employeeConfidentialFields.GetEmployeeConfidentialFields()
                          on myManager.ID equals managerInfo.ID

                          select new
                          {
                              ClientID = mainFields.ID,
                              ClientName = mainFields.Name,
                              ClientSurname = mainFields.Surname,
                              ClientBirthday = mainFields.Birthday,
                              ClientPhone = mainFields.Phone,
                              InfoID = confFields.ID,
                              Balance = confFields.Balance,
                              Credit = confFields.Credit,
                              Deposit = confFields.Deposit,
                              Password = confFields.Password,
                              ManagerName = myManager.Name,
                              ManagerSurname = myManager.Surname,
                              ManagerPhone = myManager.Phone,
                          };

            List<ClientAllData> list = new List<ClientAllData>();

            foreach (var item in allInfo)
            {
                list.Add(new ClientAllData(item.ClientID, item.ClientName, item.ClientSurname, 
                    item.ClientBirthday, item.ClientPhone, item.InfoID, item.Balance, 
                    item.Credit, item.Deposit, item.Password, item.ManagerName, 
                    item.ManagerSurname, item.ManagerPhone));
            }

            return list;
        }
    }
}