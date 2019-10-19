using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Database
{
    /// <summary>
    /// 可去https://www.connectionstrings.com/网站查
    /// </summary>
    public static class ConnectStringHelper
    {
        #region sqlserver
        public static string ConnForSqlServer(string server,string database,string userId,string password) {
            return $"Server={server};Database={database};User Id={userId};Password = {password};";
        }

        #endregion

        #region mysql
        public static string ConnForMySql(string server,string database, string userId, string password, string port="3306")
        {
            return $"Server={server};Port={port};Database={database};User Id={userId};Password = {password};";
        }
        #endregion

        #region oracle
        public static string ConnForOracleOfOdpNetWithoutOra(string server, string database, string userId, string password,string port="1521")
        {
            return $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={server})(PORT={port})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=MyOracleSID)));User Id = {userId}; Password ={password} ; ";
        }
        #endregion
    }
}
