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
    class DBClientConfidentialFields//Получение из БД информации о финансах и учетных данных клиента
    {
        #region privateFields
        private int id;
        private decimal balance;
        private decimal credit;
        private decimal deposit;
        private string password;
        #endregion

        #region fieldsProperties
        public int ID
        {
            get { return id; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public decimal Credit
        {
            get { return credit; }
        }

        public decimal Deposit
        {
            get { return deposit; }
        }

        public string Password
        {
            get { return password; }
        }
        #endregion

        public DBClientConfidentialFields() { }

        public DBClientConfidentialFields(int id, decimal balance, decimal credit, decimal deposit, string password)
        {
            this.id = id;
            this.balance = balance;
            this.credit = credit;
            this.deposit = deposit;
            this.password = password;
        }

        public List<DBClientConfidentialFields> GetClientConfidentialFields()
        {
            DBConnector connector = new DBConnector();
            string connStr = connector.GetConnectionString("Persons");
            string query = connector.QueryTable("ClientInfo");

            DataSet PersonDB = new DataSet("Persons");

            SqlDataAdapter adapter = new SqlDataAdapter(query, connStr);

            adapter.Fill(PersonDB);

            DataTable Table = PersonDB.Tables[0];

            var typeClient = from item in Table.AsEnumerable()
                             select new
                             {
                                 ID = Convert.ToInt32(item[0]),
                                 Balance = Convert.ToDecimal(item[1]),
                                 Credit = Convert.ToDecimal(item[2]),
                                 Deposit = Convert.ToDecimal(item[3]),
                                 Password = Convert.ToString(item[4])
                             };

            List<DBClientConfidentialFields> list = new List<DBClientConfidentialFields>();

            foreach (var item in typeClient)
            {
                list.Add(new DBClientConfidentialFields(item.ID, item.Balance, item.Credit, item.Deposit, item.Password));
            }

            return list;
        }
    }
}