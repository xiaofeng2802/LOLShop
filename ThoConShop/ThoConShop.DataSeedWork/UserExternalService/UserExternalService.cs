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
        static IDbConnection db =
            new SqlConnection(ConfigurationManager.ConnectionStrings["ShopThoConDb"].ConnectionString);

        public static float GetUserBalance(string generalUserId)
        {
            string query = string.Format("select TOP 1 Balance from [dbo].[User] where GeneralUserId = {0}",
                generalUserId);

            var result = db.QueryFirst<float>(query);
            return result;
        }

        public static double SetUserBalance(string generalUserId, float price)
        {

            string query = string.Format("Update [dbo].[User] SET Balance = Balance + ({0}) where GeneralUserId = {1}",
                price, generalUserId);

            var result = db.Execute(query);

            return result;
        }

        public static bool? GetUserStatus(string generalUserId)
        {
            string query = string.Format("Select TOP 1 IsActive From [dbo].[User] WHERE GeneralUserId = {0}",
                generalUserId);

            try
            {
                var result = db.QueryFirst<bool>(query);

                return result;
            }
            catch (Exception)
            {
                
            }
            return null;
        }

        public static IList<TEntity> TopChargingOfMonth<TEntity>() where TEntity : class
        {
            string query = string.Format("SELECT TOP 5 au.UserName, SUM(urh.ParValue) AS SumOfMonth "
                                         +"FROM [dbo].[User] u INNER JOIN dbo.ApplicationUsers au ON au.Id = u.GeneralUserId "
                                         + "INNER JOIN dbo.UserRechargeHistory urh ON urh.UserId = u.Id "
                                         + "WHERE MONTH(urh.CreatedDate) = MONTH(GETDATE()) "
                                         + "AND YEAR(urh.CreatedDate) = YEAR(GETDATE()) "
                                         + "AND u.IsActive = 1 "
                                         + "GROUP BY au.UserName "
                                         + "ORDER BY SumOfMonth DESC");

            try
            {
                var result = db.Query<TEntity>(query).ToList();

                return result;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static IList<TEntity> LatestUsingWheel<TEntity>() where TEntity : class
        {
            string query = string.Format("SELECT TOP 5 au.UserName, lwh.Result FROM dbo.LuckyWheelHistories lwh "
                  + "INNER JOIN [dbo].[User] u ON lwh.UserId = u.Id INNER JOIN dbo.ApplicationUsers au ON au.Id = u.GeneralUserId "
                + "ORDER BY lwh.CreatedDate DESC");

            try
            {
                var result = db.Query<TEntity>(query).ToList();

                return result;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
