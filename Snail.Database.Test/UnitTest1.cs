using System;
using Xunit;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System.Linq;

namespace Snail.Database.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Sqlserver_Test()
        {
            SqlConnection connection = new SqlConnection("");
            SqlDataAdapter adapter = new SqlDataAdapter("", "");
            
        }

        [Fact]
        public void Oracle_Test()
        {
            OracleDataAdapter adapter = new OracleDataAdapter("", "");
            adapter.Fill(new System.Data.DataSet());

        }

        [Fact]
        public void Mysql_Test()
        {
            MySqlConnection connection = new MySqlConnection();
            MySqlDataAdapter adapter = new MySqlDataAdapter("", "");
            var reader=adapter.SelectCommand.ExecuteReader();
            
        }

        [Fact]
        public void Dapper_Test()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectStringHelper.ConnForSqlServer("sqlserver.dev.rlair.net", "OrPMS", "newaoc", "newaoc")))
                {
                    var table = connection.Query("select * from T_AC_User");
                }

                using (var connection = new MySqlConnection(ConnectStringHelper.ConnForMySql("mysql.test.rlair.net", "training", "training", "training")))
                {
                    var table = connection.Query("select * from t_sys_role");
                }

                using (var connection = new OracleConnection(ConnectStringHelper.ConnForOracleOfOdpNetWithoutOra("oracle.test.rlair.net", "orcl", "B2C_OW_DBUSER", "owdbuser")))
                {
                    var table = connection.Query("select * from t_sys_role");
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
