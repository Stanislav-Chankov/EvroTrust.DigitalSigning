namespace EvroTrust.DigitalSigning.WebApi.Authz
{
    public interface ITokenProvider
    {
        /// <summary>
        /// Generates the access token.
        /// </summary>
        /// <param name="role">The role.</param>
        string GenerateAccessToken(RoleType role);

        /// <summary>
        /// Validates the access token.
        /// </summary>
        /// <param name="accessToken">The token.</param>
        bool ValidateToken(string accessToken);
    }
}