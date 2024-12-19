using Dapper;
using SV21T1020285.DomainModels;
namespace SV21T1020285.DataLayers.SQL_Server
{
    public class EmployeeAccountDAL : BaseDAL, IUserAccountDAL
    {
        public EmployeeAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select  EmployeeID as UserId, 
                                    Email as UserName, 
                                    FullName as DisplayName, 
                                    Photo, 
                                    RoleName as RoleNames
                            from Employees
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