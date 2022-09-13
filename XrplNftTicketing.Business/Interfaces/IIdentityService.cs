using System.Collections.Generic;

namespace XrplNftTicketing.Business.Interfaces
{
    public interface IIdentityService
    {
        bool Authenticate(string emailAddress, string password);
        List<string> GetUserRoles(string emailAddress);
    }
}