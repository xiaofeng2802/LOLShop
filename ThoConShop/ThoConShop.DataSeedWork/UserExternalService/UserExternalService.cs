using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
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

        public static bool IsEnoughPoint(string generalUserId)
        {
            var defaultPoint = int.Parse(ConfigurationManager.AppSettings["PointPerWheel"]);
            string query = string.Format("select TOP 1 Point from [dbo].[User] where GeneralUserId = '{0}'",
                generalUserId);

            CheckConnectionState();
            var result = db.QueryFirst<int>(query);

            if (result >= defaultPoint)
            {
                return true;
            }

            return false;
        }

        public static bool IsUnluckyItem(int id)
        {
            string query = string.Format(
                "select TOP 1 Id from [dbo].[LuckyWheelItems] where Id = {0} AND IsUnlucky = 1", id);

            try
            {
                CheckConnectionState();
                var result = db.QueryFirst<int>(query);
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }

            return false;
        }

        public static int GetUserPoint(string generalUserId)
        {

            string query = string.Format("select TOP 1 Point from [dbo].[User] where GeneralUserId = '{0}'",
                generalUserId);

            CheckConnectionState();
            var result = db.QueryFirst<int>(query);


            return result;
        }

        public static string GetDescriptonWheelItem(int id)
        {
            string query = string.Format("select TOP 1 Description from [dbo].[LuckyWheelItems] where Id = {0}", id);

            CheckConnectionState();
            var result = db.QueryFirst<string>(query);
            return result;
        }

        //public static int UpdatePointAfterUseWheel(string generalUserId)
        //{
        //    var defaultPoint = int.Parse(ConfigurationManager.AppSettings["PointPerWheel"]);
        //    string query = string.Format("Update [dbo].[User] SET Point = Point - ({0}) WHERE GeneralUserId = {1}",
        //        defaultPoint, generalUserId);

        //    var result = db.Execute(query);

        //    return result;
        //}

        public static float GetUserBalance(string generalUserId)
        {
            
            string query = string.Format("select TOP 1 Balance from [dbo].[User] where GeneralUserId = '{0}'",
                generalUserId);

            try
            {
                CheckConnectionState();
                var result = db.Query<float>(query).ToList();
                return result[0];
            }
            catch (Exception)
            {
                throw;
                return 0;
            }
        }

        //public static double SetUserBalance(string generalUserId, float price)
        //{

        //    string query = string.Format("Update [dbo].[User] SET Balance = Balance + ({0}), Point = Point + ({1}) WHERE GeneralUserId = {2}",
        //        price, (price/1000),generalUserId);

        //    var result = db.Execute(query);

        //    return result;
        //}

        public static bool? GetUserStatus(string generalUserId)
        {
            string query = string.Format("Select TOP 1 IsActive From [dbo].[User] WHERE GeneralUserId = '{0}'",
                generalUserId);

            try
            {
                CheckConnectionState();
                var result = db.QueryFirst<bool>(query);

                return result;
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        public static IList<TEntity> TopChargingOfMonth<TEntity>() where TEntity : class
        {
            string query = string.Format("SELECT TOP 5 u.NameDisplay, SUM(urh.ParValue) AS SumOfMonth "
                                         + "FROM [dbo].[User] u INNER JOIN dbo.ApplicationUsers au ON au.Id = u.GeneralUserId "
                                         + "INNER JOIN dbo.UserRechargeHistory urh ON urh.UserId = u.Id "
                                         + "WHERE MONTH(urh.CreatedDate) = MONTH(GETDATE()) "
                                         + "AND YEAR(urh.CreatedDate) = YEAR(GETDATE()) "
                                         + "AND u.IsActive = 1 "
                                         + "GROUP BY u.NameDisplay "
                                         + "ORDER BY SumOfMonth DESC");

            try
            {
                CheckConnectionState();
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
            string query = string.Format("SELECT TOP 5 u.NameDisplay, lwh.Result FROM dbo.LuckyWheelHistories lwh "
                  + "INNER JOIN [dbo].[User] u ON lwh.UserId = u.Id INNER JOIN dbo.ApplicationUsers au ON au.Id = u.GeneralUserId "
                + "ORDER BY lwh.CreatedDate DESC");

            try
            {
                CheckConnectionState();
                var result = db.Query<TEntity>(query).ToList();

                return result;
            }
            catch (Exception ex)
            {

            }
            return null;
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
