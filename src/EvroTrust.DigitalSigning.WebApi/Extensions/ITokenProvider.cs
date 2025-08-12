namespace Homeport.Domain.Services.Providers
{
    public interface ITokenProvider
    {
        string GenerateAccessToken();
        bool ValidateToken(string token);
    }
}