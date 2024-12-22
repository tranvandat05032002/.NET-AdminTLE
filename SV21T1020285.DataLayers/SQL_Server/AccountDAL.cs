using Dapper;
using System.Data;
using SV21T1020285.DomainModels;
namespace SV21T1020285.DataLayers.SQL_Server
{
    public class AccountDAL : BaseDAL, IAccountDAL
    {
        public AccountDAL(string connectionString) : base(connectionString)
        {
        }

        public Account? Get(int id)
        {
            Account? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select
                                CustomerID as AccountID,
                                CustomerName as AccountName,
                                ContactName,
                                Province,
                                [Address],
                                Phone,
                                Email,
                                Photo
                            from Customers where CustomerID = @CustomerID";
                var parameters = new
                {
                    CustomerID = id
                };
                data = connection.QueryFirstOrDefault<Account>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
            // throw new NotImplementedException();
        }

        public bool Update(Account data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Customers where CustomerId <> @CustomerId and Email = @Email)
                        begin
                            update Customers
                            set CustomerName = @CustomerName,
                                ContactName = @ContactName,
                                Province = @Province,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email,
                                Photo = @Photo
                            where CustomerID = @CustomerID 
                        end";
                var parameters = new
                {
                    CustomerID = data.AccountID,
                    CustomerName = data.AccountName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address,
                    Phone = data.Phone,
                    Email = data.Email,
                    Photo = data.Photo
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
            // throw new NotImplementedException();
        }

        public bool VerifyPassword(int userId, string password) {
            string passwordResult = null;
            using (var connection = OpenConnection())
            {
                var sql = @"
                            BEGIN
                                select [Password]
                                from Customers
                                where CustomerID = @CustomerID
                            END
                        ";
                var parameters = new 
                {
                    CustomerID = userId
                };
                passwordResult = connection.ExecuteScalar<string>(sql, parameters, commandType: CommandType.Text);
                connection.Close();
            }
            Console.WriteLine(password == passwordResult);
            return !string.IsNullOrEmpty(passwordResult) && password == passwordResult;
        }

        public bool UpdatePassword(int userId, string newPassword)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
                        begin
                            update Customers
                            set [Password] = @Password
                            where CustomerID = @CustomerID 
                        end";
                var parameters = new
                {
                    CustomerID = userId,
                    Password = newPassword
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }


        public bool ChangePassword(string username, string password)
        {
            return false;
        }
    }
}