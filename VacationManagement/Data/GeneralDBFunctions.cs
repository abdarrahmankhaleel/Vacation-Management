using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace VacationManagement.Data
{
    public static class GeneralDBFunctions
    {
        public static DataTable SqlDataTable(this DbContext context,string SqlQuery)
        {
            DbConnection conn = context.Database.GetDbConnection();
            using(var cmd = conn.CreateCommand())
            {
                cmd.CommandType=CommandType.Text;
                cmd.CommandText = SqlQuery;


                var table=new DataTable();
                if (conn.State.Equals(ConnectionState.Closed)) { conn.Open(); }

                using(var reader = cmd.ExecuteReader())
                {
                    table.Load(reader);
                }



                if (conn.State.Equals(ConnectionState.Open)) { conn.Close(); }
                return table;   
            }
        }
    }
}
