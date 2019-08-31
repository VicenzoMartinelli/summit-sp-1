using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SummitApi.Domain;
using SummitApi.DTOs;
using SummitApi.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SummitApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsuarioController : ControllerBase
  {
    private readonly IUsuarioRepository _repository;
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;

    public UsuarioController(
      IUsuarioRepository repository,
      UserManager<Usuario> userManager,
      SignInManager<Usuario> signInManager)
    {
      _repository = repository;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    [HttpGet]
    [Authorize]
    public List<Usuario> ObterUsuarios()
    {
      return _repository.ObterTodos();
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegistrarViewModel registerUser)
    {
      var user = new Usuario
      {
        UserName    = registerUser.UserName,
        Email       = registerUser.Email,
        ApelidoFofo = registerUser.ApelidoFofo
      };

      var result = await _userManager.CreateAsync(user, registerUser.Password);

      if (!result.Succeeded)
      {
        return BadRequest();
      }

      await _signInManager.SignInAsync(user, false);

      return new OkObjectResult(await GerarJwt(user.UserName));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogInAsync([FromBody] LoginViewModel login)
    {
      var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, true);

      if (!result.Succeeded)
      {
        return BadRequest();
      }

      return new OkObjectResult(await GerarJwt(login.UserName));
    }

    private async Task<string> GerarJwt(string username)
    {
      var user = await _userManager.FindByNameAsync(username);

      var identityClaims = new ClaimsIdentity();
      
      identityClaims.AddClaims(await _userManager.GetClaimsAsync(user));

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes("SponteSummit2019");
      var dataCriacao = DateTime.UtcNow;
      var valid = dataCriacao.AddHours(24);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject            = identityClaims,
        Expires            = valid,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
  }
}