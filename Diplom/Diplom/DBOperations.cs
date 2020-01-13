using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace Diplom
{
    class DBOperations
    {
        #region privateFields
        private int id;
        private string name;
        private string surname;
        private string birthday;
        private string phone;
        private int manager;
        #endregion

        #region fieldsProperties
        public int ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Surname
        {
            get { return surname; }
        }

        public string Birthday
        {
            get { return birthday; }
        }

        public string Phone
        {
            get { return phone; }
        }

        public int Manager
        {
            get { return manager; }
        }
        #endregion

        public DBOperations(int id, string name, string surname, string birthday, string phone, int manager)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.birthday = birthday;
            this.phone = phone;
            this.manager = manager;
        }

        public DBOperations(int id, string name, string surname, string birthday, string phone)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.birthday = birthday;
            this.phone = phone;
        }

        public string Intro()
        {
            return $"ID - {ID}, {Name} {Surname}\nДата Рождения - {Birthday}, Телефон - {Phone}";
        }

        public DBOperations() { }

        public List<DBOperations> GetEmployeeFields()
        {
            DBConnector connector = new DBConnector();
            string connStr = connector.GetConnectionString("Persons");
            string query = connector.QueryTable("Employees");

            DataSet PersonDB = new DataSet("Persons");

            SqlDataAdapter adapter = new SqlDataAdapter(query, connStr);

            adapter.Fill(PersonDB);

            DataTable parentTable = PersonDB.Tables[0];

            var typeClient = from item in parentTable.AsEnumerable()
                             select new
                             {
                                 ID = Convert.ToInt32(item[0]),
                                 Name = item[1].ToString(),
                                 Surname = Convert.ToString(item[2]),
                                 Birthday = Convert.ToDateTime(item[3]).ToShortDateString(),
                                 Phone = Convert.ToString(item[4])
                             };

            List<DBOperations> list = new List<DBOperations>();

            foreach (var item in typeClient)
            {
                list.Add(new DBOperations(item.ID, item.Name, item.Surname, item.Birthday, item.Phone));
            }

            return list;
        }

        public List<DBOperations> GetClientFields()
        {
            DBConnector connector = new DBConnector();
            string connStr = connector.GetConnectionString("Persons");
            string query = connector.QueryTable("Clients");

            DataSet PersonDB = new DataSet("Persons");

            SqlDataAdapter adapter = new SqlDataAdapter(query, connStr);

            adapter.Fill(PersonDB);

            DataTable Table = PersonDB.Tables[0];

            var typeClient = from item in Table.AsEnumerable()
                             select new
                             {
                                 ID = Convert.ToInt32(item[0]),
                                 Name = item[1].ToString(),
                                 Surname = Convert.ToString(item[2]),
                                 Birthday = Convert.ToDateTime(item[3]).ToShortDateString(),
                                 Phone = Convert.ToString(item[4]),
                                 Manager = Convert.ToInt32(item[5])
                             };

            List<DBOperations> list = new List<DBOperations>();

            foreach (var item in typeClient)
            {
                list.Add(new DBOperations(item.ID, item.Name, item.Surname, item.Birthday, item.Phone, item.Manager));
            }

            return list;
        }
    }
}