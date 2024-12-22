using SV21T1020285.DataLayers;
using SV21T1020285.DataLayers.SQL_Server;
using SV21T1020285.DomainModels;
namespace SV21T1020285.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL EmployeeAccountDB;
        private static readonly IUserAccountDAL CustomerAccountDB;
        private static readonly IAccountDAL AccountDB;
        static UserAccountService()
        {
            String connectionString = Configuration.ConnectionString;
            EmployeeAccountDB = new EmployeeAccountDAL(connectionString);
            CustomerAccountDB = new CustomerAccountDAL(connectionString);
            AccountDB = new AccountDAL(connectionString);
        }

        public static UserAccount? Authorize(UserTypes userType, string username, string password)
        {
            if (userType == UserTypes.Employee)
            {
                return EmployeeAccountDB.Authorize(username, password);
            }
            else
            {
                return CustomerAccountDB.Authorize(username, password);
            }
        }

        public static Account? GetAccount(int id)
        {
            return AccountDB.Get(id);
        }

        // Account from Customer. UI for Customer
        public static bool Update(Account data)
        {
            return AccountDB.Update(data);
        }

        public static bool VerifyPassword(int userId, string password) {
            return AccountDB.VerifyPassword(userId, password);
        }
        public static bool UpdatePassword(int userId, string newPassword) {
            return AccountDB.UpdatePassword(userId, newPassword);
        }
    }

    public enum UserTypes
    {
        Employee,
        Customer
    }
}