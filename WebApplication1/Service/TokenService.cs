using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using AuthenticationService.Model;

using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Service
{
    public class TokenService
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
                Expires = DateTime.UtcNow.AddMinutes(4), //expira a cada 2 horas cada token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // chave encriptada
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); // gerando o token
            return tokenHandler.WriteToken(token);


        }
        public static string GenerateToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor // vai conter as informações do token
            {
                
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1), //expira a cada 2 horas cada token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // chave encriptada

            };
            var token = tokenHandler.CreateToken(tokenDescriptor); // gerando o token
            return tokenHandler.WriteToken(token); // retornando no formato string
        }
        public static string GenerateRefreshToken() // gera uma string unica
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create(); // Gera um numero randomico
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber); // retorna para base 64
        }
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token) // metodo para extrair os claims de dentro do token
        {
            var tokenValidationParameters = new TokenValidationParameters // Gerando os parametros para gerar o token
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret)),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase)) // se a chave for diferenta para descriptar o token, irá dar o excpetion com invalid token
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private static List<(string, string)> _refreshTokens = new(); // lista para armazenar o token
    
        public static void SaveRefreshToken(string username, string refreshToken) // salva na lista
        {
            _refreshTokens.Add(new(username,refreshToken));
        }

        public static string GetRefreshToken(string username) // pega o refreshToken
        {
            return _refreshTokens.FirstOrDefault(x => x.Item1 == username).Item2;
        }

        public static void DeleteRefreshToken(string username, string refreshToken) // Exclui refreshtoken antigo
        {
            var item = _refreshTokens.FirstOrDefault(x => x.Item1 == username && x.Item2 == refreshToken);
            _refreshTokens.Remove(item);
        }
    }
}
