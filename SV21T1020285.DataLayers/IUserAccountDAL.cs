using SV21T1020285.DomainModels;

namespace SV21T1020285.DataLayers
{
    public interface IUserAccountDAL
    {
        UserAccount? Authorize(string username, string password);

        bool ChangePassword(string username, string password);

        int Register(string fullName, string email, string password);
    }
}
