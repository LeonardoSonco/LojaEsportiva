using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using InventoryService.Models;

using Microsoft.IdentityModel.Tokens;

namespace InventoryService.Services
{
    public static class TokenService
    {   
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor // vai conter as informações do token
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2), //expira a cada 2 horas cada token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature) // chave encriptada

            };
            var token = tokenHandler.CreateToken(tokenDescriptor); // gerando o token
            return tokenHandler.WriteToken(token); // retornando no formato string
        }
       
    }
}
