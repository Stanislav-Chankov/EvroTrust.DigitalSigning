using EvroTrust.DigitalSigning.WebApi.Authz;
using EvroTrust.DigitalSigning.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvroTrust.DigitalSigning.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenProvider _tokenProvider;

        public AuthController(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult GetToken([FromBody] LoginModel login)
        {
            return Ok(new { token = _tokenProvider.GenerateAccessToken(login.Role) });
        }
    }
}