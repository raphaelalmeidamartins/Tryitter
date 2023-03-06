using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Tryitter.Models;

namespace Tryitter.Utils;

public class TokenGenerator : ITokenGenerator
{
  public string Generate(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor()
    {
      Subject = this.AddClaims(user),
      Expires = DateTime.Now.AddDays(1),
      SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.ASCII.GetBytes("2d74025e7bcf058897d8daaa99ae99b5")),
        SecurityAlgorithms.HmacSha256Signature
      ),
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
  private ClaimsIdentity AddClaims(User user)
  {
    var claims = new ClaimsIdentity(); ;
    claims.AddClaim(new Claim("Id", user.UserId.ToString()));
    claims.AddClaim(new Claim("Email", user.Email.ToString()));

    if (user.IsAdmin)
    {
      claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
    }
    else
    {
      claims.AddClaim(new Claim(ClaimTypes.Role, "User"));
    }

    return claims;
  }
}
