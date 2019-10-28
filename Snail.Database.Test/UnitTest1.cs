using System;
using Xunit;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Snail.Common;
using System.Threading.Tasks;

namespace Snail.Database.Test
{
    public class UnitTest1
    {
        #region MyRegion
        public string pubKey = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxMu43LG4aJNn4ZVmIGXZ
u1l3p2bL3d4cH4lTw3bgwSLiya8wAy3QLYACy39dov92MrvCDkKyJ//2jp3BXyQV
f/s9f7q3tFIj3kkVLMD3C+rpcScRv7e92UmP4jsg/Z5ocOIxzqZDP4KV6e5Kzp7p
zYzxVjTNrEulMsnfeJLRV7psYx2ywnwV+f7A+OW4kWObcIuT1Ex1iDPX2qZS5lSb
57vlgAjiHFKdVDyiTLR6hTe+xtcjKgWl7voDyim8W1cp6uYV3L4clfeiKJBLrf4M
PChRVHzNOFgpqXHsUmdi/0VnsAr+LjnKyygJPBOxwBRtfHqmWdED1xNv4nlseMyG
ZQIDAQAB
-----END PUBLIC KEY-----
";
        public string priKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAxMu43LG4aJNn4ZVmIGXZu1l3p2bL3d4cH4lTw3bgwSLiya8w
Ay3QLYACy39dov92MrvCDkKyJ//2jp3BXyQVf/s9f7q3tFIj3kkVLMD3C+rpcScR
v7e92UmP4jsg/Z5ocOIxzqZDP4KV6e5Kzp7pzYzxVjTNrEulMsnfeJLRV7psYx2y
wnwV+f7A+OW4kWObcIuT1Ex1iDPX2qZS5lSb57vlgAjiHFKdVDyiTLR6hTe+xtcj
KgWl7voDyim8W1cp6uYV3L4clfeiKJBLrf4MPChRVHzNOFgpqXHsUmdi/0VnsAr+
LjnKyygJPBOxwBRtfHqmWdED1xNv4nlseMyGZQIDAQABAoIBABsDOrYCaAVkvGTc
yLaoPFWIz5GJHPEwqGwUwcxqACpKk1YrR5wcGP/x2xBbRHtX9P28Q4QJGCLA8fM8
CLu6PIBDeHrUopwsDTPZFMC+oPqADXiEbB2Eh1jwzcD4LExbxsq95afPrxnj3xeL
57VKX13hW4whddzRSlT5HEVU48rWdF01Bc2Rdc5oB0uQTONWgMfbJeBvo++iEyjc
N/Jqeb4Omwu1PRPzS/Q+guC2Sjg7CN5eCWgda1x4kRrF8e5IN5/xOM/Irffdnw07
7hxfzj2OJmND/UpgWZtLgVukewZZo72fXfNHEhwT9PIqo8hpQwgSPuC9Ud6Vcmt+
hipIZAECgYEA9vRZ5UpVkpIPj2TBJS+6xcbbs2kpvggx5bQM1DlIQBatItPpITJT
JO6MfBj0LX9i53AFqfGJXG38wS0Cbtuz6Nzr+2JDXe6hrUulFTo/mkPtwL0ptu95
UReXjVTzNISbCcVPhhSSVvAgLs0pFi9ZDevJbi6KH/uhcDyEXQuSvAECgYEAzAEK
pfIkU1vQ5P7zH9h9Dk/wo+ELEmpJbHxnUJlM+2SuHVK4/br0GNPOjZGtwzNe+uLF
CzoDUDdHP6Fe+YqW+/uXJezqfd41BqVphwWfFNLm+n/g9lSMZ2rFIUgz+6E20XLO
xpcNnMm7xwzIuuG+PffjCyKHEQ0S71nG8JHQWmUCgYBMDJeaarfLeTtddzOblgU4
XrLNnzcBlFh5WmcQ+8rqIZGTxhpm5K6CEwwkMzMOx8nXZ8H2wbEBS8WoX4n+RZ4z
ucTaFzqTtKcJTOA7l0J66SxQTHCKK1j6xf8fwOdcZvGvopmIutEOAMiIYRmkAVS1
WsUfLynOC5l9jMVeOfAoAQKBgQDDjLgNTCf/88Iw0CZzP0zYvE4aeOzpERMit7k9
LEX7sI2qNBJ5vYygg9+6Gouq0oJYEan50fk9Gk/ksaXdpDiIgKlpREmer7K6lTKr
p/rOtj+Mnaoh1ffkZhdiiNizetyWNuv4tvDoewPRkPKVGTEIK6bqlIVOFe8xmig1
kEBddQKBgF59StlJ4wqWnRAdtXIgE8iLxbA9jWqjvs4MlL0p6iXdEUQlOrUxymNI
lE0kf4cyrR/2hvlKGkiCr+wivGTaIbzowdv0K/cv/urDKPjmFqYIOZihZtHtWiJU
q68H0YAe96orUsuZe9eT3cVfxZdICiowDp4+UvJTR1bvtFk13lJV
-----END RSA PRIVATE KEY-----
";
        #endregion
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
            var reader = adapter.SelectCommand.ExecuteReader();

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
                    var table = connection.Query("select * from t_user");
                }
            }
            catch (Exception ex)
            {
            }
        }

        [Fact]
        public void Jwt()
        {
            try
            {
                IdentityModelEventSource.ShowPII = true;
                var cred = GetSigningCredentials("rs256");
                var jwt = new JwtSecurityToken("issuer", "audience", new List<Claim> { new Claim("name", "zhoujing") }, null, DateTime.Now.AddHours(1), cred);
                var tokenStr = new JwtSecurityTokenHandler().WriteToken(jwt);

                new JwtSecurityTokenHandler().ValidateToken(tokenStr, new TokenValidationParameters {
                    IssuerSigningKeys=new List<SecurityKey> {
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890abcdefghij")),
                        new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPrivatePem(priKey))
                    },
                    ValidateAudience=false,
                    ValidateIssuer=false
                }, out SecurityToken securityToken);
            }
            catch (Exception ex)
            {

            }
        }

        private SigningCredentials GetSigningCredentials(string alg)
        {
            SigningCredentials cred = null;
            SecurityKey key;
            try
            {
                if (alg == "hm256")
                {
                    key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890abcdefghij"));
                    cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                }
                if (alg == "rs256")
                {
                    key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPrivatePem(priKey));
                    cred = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
            return cred;

        }

        [Fact]
        public void LockStringTest()
        {
            try
            {
                var lockStr = "lock";
                var count = 0;
                var tasks = new List<Task>();
                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        lock (lockStr)
                        {
                            for (int j = 0; j < 10000; j++)
                            {
                                count = count + 1;
                            }
                            LockA();

                        }

                    }));
                }
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                var ss = ex;
            }
        }
        private static int c = 0;
        private void LockA()
        {
            Task.Run(()=> { 
             lock ("lock")
            {
                c = c + 1;
            }
            });
           
        }
    }
}
