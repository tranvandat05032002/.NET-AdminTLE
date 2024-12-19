using SV21T1020285.DataLayers;
using SV21T1020285.DataLayers.SQL_Server;
using SV21T1020285.DomainModels;
namespace SV21T1020285.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL EmployeeAccountDB;
        private static readonly IUserAccountDAL CustomerAccountDB;
        static UserAccountService()
        {
            String connectionString = Configuration.ConnectionString;
            EmployeeAccountDB = new EmployeeAccountDAL(connectionString);
            CustomerAccountDB = new CustomerAccountDAL(connectionString);
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
    }

    public enum UserTypes
    {
        Employee,
        Customer
    }
}