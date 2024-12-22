using Dapper;
using SV21T1020285.DomainModels;
namespace SV21T1020285.DataLayers.SQL_Server
{
    public class CustomerAccountDAL : BaseDAL, IUserAccountDAL
    {
        public CustomerAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select  CustomerID as UserId, 
                                    Email as UserName, 
                                    CustomerName as DisplayName, 
                                    Photo, 
                                    N'' as RoleNames
                            from Customers
                            where Email = @Email and [Password] = @Password";
                var parameters = new
                {
                    Email = username,
                    Password = password,
                };
                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }

            return data;
        }

        public bool ChangePassword(string username, string password)
        {
            return false;
        }
    }
}