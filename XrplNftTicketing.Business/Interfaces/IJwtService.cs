namespace XrplNftTicketing.Business.Interfaces
{
    public interface IJwtService
    {
        string GenerateSecurityToken(string email, string password);
    }
}