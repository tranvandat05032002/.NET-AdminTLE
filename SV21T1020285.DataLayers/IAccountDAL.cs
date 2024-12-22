using SV21T1020285.DomainModels;

namespace SV21T1020285.DataLayers
{
    public interface IAccountDAL
    {
        Account? Get(int id);
        bool Update(Account data);
        bool VerifyPassword(int userId, string password);
        bool UpdatePassword(int userId, string newPassword);
        bool ChangePassword(string username, string password);
    }
}