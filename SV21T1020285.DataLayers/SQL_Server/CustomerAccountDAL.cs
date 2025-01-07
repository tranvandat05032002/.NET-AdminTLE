using System.Data;
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

        public int Register(string fullName, string email, string password)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
            insert into Customers(CustomerName, ContactName, Email, [Password], IsLocked)
            values(@CustomerName, @CustomerName, @Email, @Password, @IsLocked)
            select scope_identity();
            ";

                var parameters = new
                {
                    CustomerName = fullName ?? "",
                    ContactName = fullName ?? "",
                    Password = password,
                    Province = "",
                    Address = "",
                    Phone = "",
                    Email = email ?? "",
                    IsLocked = false
                };
                id = connection.ExecuteScalar<int>(sql, parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return id;
        }

        public bool ChangePassword(string username, string password)
        {
            return false;
        }
    }
}