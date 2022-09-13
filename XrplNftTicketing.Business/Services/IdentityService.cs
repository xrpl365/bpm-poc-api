using System;
using System.Collections.Generic;
using XrplNftTicketing.Business.Interfaces;

namespace XrplNftTicketing.Business.Services
{
    /// <summary>
    /// User - Identity Management Service
    /// Will connect through to user data repository
    /// </summary>
    public class IdentityService : IIdentityService
    {

        public bool Authenticate(string emailAddress, string password)
        {
            return true;
        }
        public List<string> GetUserRoles(string emailAddress)
        {
            if (emailAddress == "agency@myageny.com")
                return new List<string>() { "Agency" };
            if (emailAddress == "user@test.com")
                return new List<string>() { "User" };

            return null;
        }



    }
}
