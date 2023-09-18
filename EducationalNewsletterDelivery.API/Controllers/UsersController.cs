using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EducationalNewsletterDelivery.API.Models;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.Repository.IRepositories;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalNewsletterDelivery.API.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : BaseController<UsersController>
    {

        public UsersController(IUnitOfWork unitOfWork,
                          IMapper mapper,
                          ILogger<UsersController> logger
                          ) : base(unitOfWork, mapper, logger)
        {
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PromoteUserToAdminAsync(int id)
        {
            try
            {
                if (!await _unitOfWork.UserRepository.ExistUserByIdAsync(id))
                {
                    return BadRequest("The user does not exist.");
                }
                await _unitOfWork.UserRepository.PromoteUserToAdminRoleAsync(id);
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, _defaultBackendErrorMessage);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DemoteUserToUserAsync(int id)
        {
            try
            {
                if (!await _unitOfWork.UserRepository.ExistUserByIdAsync(id))
                {
                    return BadRequest("The user does not exist.");
                }
                await _unitOfWork.UserRepository.DemoteUserToUserRoleAsync(id);
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, _defaultBackendErrorMessage);
            }

        }
    }
}