using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nenita.Data
{
    internal class DBConnection
    {
        private readonly string connectionString;

        public DBConnection()
        {
            connectionString = @"Data Source=DEYDEY\SQLSERVERY;Initial Catalog=db_CRUMAR;Integrated Security=True";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

    }
}
