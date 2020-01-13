using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace Diplom
{
    interface INewClient
    {
        int ClientID { get; }
        string Name { get; }
        string Surname { get; }
        string Birthday { get; }
        string Phone { get; }
        int Manager { get; }
        int InfoID { get; }
        decimal Balance { get; }
        decimal Credit { get; }
        decimal Deposit { get; }
        string Password { get; }
    }

    class CreateClient : INewClient
    {
        #region privateFields
        private int clientID;
        private string name;
        private string surname;
        private string birthday;
        private string phone;
        private int manager;
        private int infoID;
        private decimal balance;
        private decimal credit;
        private decimal deposit;
        private string password;
        #endregion

        #region fieldsProperties
        public int ClientID { get { return clientID; } }

        public string Name{ get { return name; } }

        public string Surname{ get { return surname; } }

        public string Birthday { get { return birthday; } }

        public string Phone { get { return phone; } }

        public int Manager { get { return manager; } }

        public int InfoID { get { return infoID; } }

        public decimal Balance { get { return balance; } }

        public decimal Credit { get { return credit; } }

        public decimal Deposit { get { return deposit; } }

        public string Password { get { return password; } }
        #endregion

        public CreateClient(int clientID, string name, string surname, string birthday, string phone, int manager)
        {
            this.clientID = clientID;
            this.name = name;
            this.surname = surname;
            this.birthday = birthday;
            this.phone = phone;
            this.manager = manager;
        }

        public CreateClient(int infoID, decimal balance, decimal credit, decimal deposit, string password)
        {
            this.infoID = infoID;
            this.balance = balance;
            this.credit = credit;
            this.deposit = deposit;
            this.password = password;
        }

        public CreateClient() { }

        private List<CreateClient> GetNewClient()
        {
            DBOperations operations = new DBOperations();
            
            List<CreateClient> list = new List<CreateClient>();

            int clientID = 0;
            foreach(var item in operations.GetClientFields())
            {
                clientID = item.ID + 1;
            }
            Console.Write("Имя нового клиента - ");
            string name = Console.ReadLine();

            Console.Write("Фамилия клиента - ");
            string surname = Console.ReadLine();

            Console.Write("Дата рождения в формате ГГГГ-ММ-ДД - ");
            string birthday = Console.ReadLine();

            Console.Write("Телефон клиента в формате (ХХХ)ХХХ-ХХ-ХХ - ");
            string phone = Console.ReadLine();
            string formatPhone = string.Format("{0:(0##)###-##-##}", Convert.ToInt64(phone));

            int manager = 0;
            while (true)
            {
                Console.Write("ID Менеджера - ");
                string inputManager = Console.ReadLine();
                bool resultInput = int.TryParse(inputManager, out manager);

                if (resultInput == false)
                {
                    Console.WriteLine("\nОшибка! Введите ID менеджера!\n");
                    continue;
                }

                break;
            }

            this.clientID = clientID;
            this.name = name;
            this.surname = surname;
            this.birthday = birthday;
            this.phone = formatPhone;
            this.manager = manager;

            list.Add(new CreateClient(ClientID, Name, Surname, Birthday, Phone, Manager));

            return list;
        }

        private List<CreateClient> GetAdditionalInfo()
        {
            DBClientConfidentialFields confidentialFields = new DBClientConfidentialFields();

            List<CreateClient> list = new List<CreateClient>();

            int infoID = 0;
            decimal balance = 0;
            decimal credit = 0;
            decimal deposit = 0;
            string password = null;

            foreach(var item in confidentialFields.GetClientConfidentialFields())
            {
                infoID = item.ID + 1;
            }

            CheckCorrectInput("Сумма на счету", out balance);
            CheckCorrectInput("Сумма кредита", out credit);
            CheckCorrectInput("Сумма депозита", out deposit);

            Console.Write("Пароль для входа - ");
            password = Console.ReadLine();

            this.infoID = infoID;
            this.balance = balance;
            this.credit = credit;
            this.deposit = deposit;
            this.password = password;

            list.Add(new CreateClient(InfoID, Balance, Credit, Deposit, Password));

            return list;
        }

        public void AddNewCLient()
        {
            DBConnector connector = new DBConnector();

            string connStr = connector.GetConnectionString("Persons");
            string query = null;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                foreach (var item in GetNewClient())
                {
                    query = $"insert into Clients(ClientID, ClientName, ClientSurname, ClientBirthday, ClientPhone, ManagerID) values({item.ClientID}, '{item.Name}', '{item.Surname}', '{item.Birthday}', '{item.Phone}', {item.Manager})";
                }

                SqlCommand command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                foreach (var item in GetAdditionalInfo())
                {
                    query = $"insert into ClientInfo(ID, Balance, Credit, Deposit, Password) values({item.InfoID}, {item.Balance}, {item.Credit}, {item.Deposit}, '{item.Password}')";
                }

                SqlCommand command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        private void CheckCorrectInput(string accountTypeName, out decimal accountType)
        {
            while (true)
            {
                Console.Write($"{accountTypeName} - ");
                var input = Console.ReadLine();
                bool inputResult = decimal.TryParse(input, out accountType);

                if (inputResult == false)
                {
                    Console.WriteLine("\nОшибка! Введите число!\n");
                    continue;
                }

                break;
            }
        }
    }
}