using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ThoConShop.DataSeedWork.UserExternalService
{
    public class UserExternalService
    {
        static IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopThoConDb"].ConnectionString);

        public static double GetUserBalance(string generalUserId)
        {
            string query = string.Format("select TOP 1 Balance from [dbo].[User] where GeneralUserId = {0}", generalUserId);

            var result = db.QueryFirst<double>(query);
            return result;
        }

        public static double SetUserBalance(string generalUserId, decimal price)
        {

            string query = string.Format("Update [dbo].[User] SET Balance = Balance + ({0}) where GeneralUserId = {1}", price, generalUserId);

            var result = db.Execute(query);

            return result;
        }

        public static bool GetUserStatus(string generalUserId)
        {
            string query = string.Format("Select TOP 1 IsActive From [dbo].[User] WHERE GeneralUserId = {0}", generalUserId);

            var result = db.QueryFirst<bool>(query);

            return result;
        }
    }
}
