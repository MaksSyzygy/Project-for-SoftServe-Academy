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
    class DBEmployeeConfidentialFields//Получение из БД информации о зарплате и должности сотрудника
    {
        #region privateFields
        private int id;
        private decimal salary;
        private string position;
        #endregion

        #region fieldsProperties
        public int ID
        {
            get { return id; }
        }

        public decimal Salary
        {
            get { return salary; }
        }

        public string Position
        {
            get { return position; }
        }
        #endregion

        private DBEmployeeConfidentialFields(int id, decimal salary, string position)
        {
            this.id = id;
            this.salary = salary;
            this.position = position;
        }

        public DBEmployeeConfidentialFields() { }

        public List<DBEmployeeConfidentialFields> GetEmployeeConfidentialFields()
        {
            DBConnector connector = new DBConnector();
            string connStr = connector.GetConnectionString("Persons");
            string query = connector.QueryTable("EmployeeInfo");

            DataSet PersonDB = new DataSet("Persons");

            SqlDataAdapter adapter = new SqlDataAdapter(query, connStr);

            adapter.Fill(PersonDB);

            DataTable Table = PersonDB.Tables[0];

            var typeClient = from item in Table.AsEnumerable()
                             select new
                             {
                                 ID = Convert.ToInt32(item[0]),
                                 Salary = Convert.ToDecimal(item[1]),
                                 Position = item[2].ToString(),
                             };

            List<DBEmployeeConfidentialFields> list = new List<DBEmployeeConfidentialFields>();

            foreach (var item in typeClient)
            {
                list.Add(new DBEmployeeConfidentialFields(item.ID, item.Salary, item.Position));
            }

            return list;
        }
    }
}