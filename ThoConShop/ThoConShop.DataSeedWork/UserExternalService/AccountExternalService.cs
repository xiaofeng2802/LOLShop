using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.DataSeedWork.UserExternalService
{
    public class AccountExternalService
    {

        static IDbConnection db =
           new SqlConnection(ConfigurationManager.ConnectionStrings["ShopThoConDb"].ConnectionString);

        public static void UpdateAllAccountEventPrice(int allEventPrice)
        {
            string query = string.Format("Update dbo.Account SET EventPrice = {0} ", allEventPrice);

            try
            {
                CheckConnectionState();
                db.Execute(query);
            }
            catch (Exception)
            {
            }
        }

        private static void CheckConnectionState()
        {
            if (db.State == ConnectionState.Open)
            {
                db.Close();
            }
        }
    }
}
