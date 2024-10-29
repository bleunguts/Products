using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Products.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController: ControllerBase
{
    private string _jwtKey;
    private string _jwtIssuer;
    private string _jwtAudience;

    public AuthController(IOptions<ApplicationSettings> options)
    {
        _jwtKey = options.Value.JwtKey;
        _jwtIssuer = options.Value.JwtIssuer;
        _jwtAudience = options.Value.JwtAudience;
    }

    [AllowAnonymous]
    [HttpGet]
    public ActionResult<string> GetJwtBearerKey()
    {
        // TODO: add simple header check here with a cleartext u/p ??
        var key = Encoding.UTF8.GetBytes(_jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim("Id", Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _jwtIssuer,
            Audience = _jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(tokenString);
    }
}
