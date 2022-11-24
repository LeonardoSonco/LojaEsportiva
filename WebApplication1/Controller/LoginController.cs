using AuthenticationService.Model;
using AuthenticationService.Repositories;
using AuthenticationService.Service;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationService.Controller
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public ActionResult<dynamic> Authenticate([FromBody] UserLogin model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuario invalido" });

            var token = TokenService.GenerateToken(user);
            var refreshToken = TokenService.GenerateRefreshToken(); // gerando token de atualização
            TokenService.SaveRefreshToken(user.Username, refreshToken); // salvando esse token na memoria
            user.Password = "";
            return new
            {
                user = user,
                token = token,
                refreshToken = refreshToken
            };
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] Refresh refresh)
        {
            var principal = TokenService.GetPrincipalFromExpiredToken(refresh.Token); // pega os claims do token
            var username = principal.Identity.Name; // pega a informação do nome dentro do claim
            var savedRefreshToken = TokenService.GetRefreshToken(username); // pega da lista o refresh token

            if (savedRefreshToken != refresh.RefreshToken)
            {
                Console.WriteLine(savedRefreshToken);
                Console.WriteLine(refresh.RefreshToken);
                throw new SecurityTokenException("Invalido o refreshtoken");
            }
                
         
            var newJwtToken = TokenService.GenerateToken(principal.Claims); // gera um novo token
            var newRefreshToken = TokenService.GenerateRefreshToken(); // gera um novo refresh token
            TokenService.DeleteRefreshToken(username, refresh.RefreshToken); // delete o refresh token que foi usado para criar um novo token
            TokenService.SaveRefreshToken(username, newRefreshToken); // salve o token novo
            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
                
            });
        }

        [HttpGet]
        [Route("refresh")]
        [Authorize]
        public string Authenticated() => $"Autenticado - {User.Identity.Name}";
    }
}
