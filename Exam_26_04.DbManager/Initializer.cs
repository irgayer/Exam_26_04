using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_26_04.DbManager
{
    public abstract class Initializer
    {
        protected readonly string connectionString;
        protected SqlTransaction transaction;
        protected SqlConnection connection;

        public Initializer()
        {
            connectionString = ConfigurationManager.ConnectionStrings["appConnection"].ConnectionString;
        }
    }
}
