using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EducationalNewsletterDelivery.API.Models;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.Repository.IRepositories;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EducationalNewsletterDelivery.API.Controllers
{
  [ApiController]
  [Route("[controller]/[Action]")]
  public class AuthController : BaseController<AuthController>
  {

    public AuthController(IUnitOfWork unitOfWork,
                      IMapper mapper,
                      ILogger<AuthController> logger,
                      IConfiguration configuration
                      ) : base(unitOfWork, mapper, logger, configuration)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Register(AuthUserDTO authUserDTO)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        if (await _unitOfWork.UserRepository.ExisUserBytUsername(authUserDTO.Username))
        {
          return BadRequest("The username has already exist.");
        }
        User user = _mapper.Map<User>(authUserDTO);
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveAsync();
        var token = createToken(user);
        return Ok(token);

      }
      catch (Exception ex)
      {
        LogError(ex);
        return StatusCode(500, _defaultBackendErrorMessage);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Login(AuthUserDTO authUserDTO)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        User? user = await _unitOfWork.UserRepository.GetUserByUsernameAndPassword(authUserDTO.Username, authUserDTO.Password);
        if (user == null)
        {
          return BadRequest("The username or password is wrong.");
        }
        var token = createToken(user);
        return Ok(token);

      }
      catch (Exception ex)
      {
        LogError(ex);
        return StatusCode(500, _defaultBackendErrorMessage);
      }
    }

    private string createToken(User user)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken(
        null,
        null,
        claims: new List<Claim>()
        {
                new Claim("Id", user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("Role", user.Role.ToString()),
        },
        null,
        expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:expires"])),
        signingCredentials: credentials);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }

  }
}