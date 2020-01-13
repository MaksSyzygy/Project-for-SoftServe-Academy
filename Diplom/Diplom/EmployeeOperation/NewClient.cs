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
}