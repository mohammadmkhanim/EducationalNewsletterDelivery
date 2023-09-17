using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EducationalNewsletterDelivery.API.Models;
using EducationalNewsletterDelivery.API.Services;
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
    public async Task<IActionResult> RegisterAsync(AuthUserDTO authUserDTO)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        if (await _unitOfWork.UserRepository.ExisUserBytUsernameAsync(authUserDTO.Username))
        {
          return BadRequest("The username has already exist.");
        }
        authUserDTO.Password = HashService.GenerateSha256Hash(authUserDTO.Password);
        User user = _mapper.Map<User>(authUserDTO);
        user.Role = Role.User;
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
    public async Task<IActionResult> LoginAsync(AuthUserDTO authUserDTO)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        authUserDTO.Password = HashService.GenerateSha256Hash(authUserDTO.Password);
        User? user = await _unitOfWork.UserRepository.GetUserByUsernameAndPasswordAsync(authUserDTO.Username, authUserDTO.Password);
        if (user == null)
        {
          return BadRequest("The username or password is wrong.");
        }
        await ReceiveNewslettersAsync(user.Id);
        var token = createToken(user);
        return Ok(token);

      }
      catch (Exception ex)
      {
        LogError(ex);
        return StatusCode(500, _defaultBackendErrorMessage);
      }
    }

    private async Task ReceiveNewslettersAsync(int userId)
    {
      var userDeliveredNewsletters = await _unitOfWork.DeliveredNewsletterRepository.GetAsync(d => d.UserId == userId);
      foreach (var userDeliveredNewsletter in userDeliveredNewsletters)
      {
        if (!await _unitOfWork.DeliveredNewsletterRepository.IsDeliveredNewsletterReceivedAsync(userDeliveredNewsletter.Id))
        {
          await _unitOfWork.DeliveredNewsletterRepository.MarkDeliveredNewsletterAsReceivedAsync(userDeliveredNewsletter.Id);
        }
      }
      await _unitOfWork.SaveAsync();
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
                new Claim("role", user.Role.ToString()),
        },
        null,
        expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:expires"])),
        signingCredentials: credentials);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}