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
    [Authorize]
    public class DeliveryController : BaseController<DeliveryController>
    {
        public DeliveryController(IUnitOfWork unitOfWork,
                          IMapper mapper,
                          ILogger<DeliveryController> logger
                          ) : base(unitOfWork, mapper, logger)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Send(SendNewsletterDTO sendNewsletterDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (!await _unitOfWork.NewsletterRepository.ExistNewsletter(sendNewsletterDTO.NewsletterId))
                {
                    return BadRequest("The newsletter does not exist.");
                }
                foreach (var userId in sendNewsletterDTO.UserIds)
                {
                    if (await _unitOfWork.UserRepository.ExistUserById(userId))
                    {
                        DeliveredNewsletter deliveredNewsletter = new DeliveredNewsletter()
                        {
                            DeliveredDateTime = DateTime.Now,
                            NewsletterId = sendNewsletterDTO.NewsletterId,
                            UserId = userId
                        };
                        await _unitOfWork.DeliveredNewsletterRepository.AddAsync(deliveredNewsletter);
                    }
                    else
                    {
                        return BadRequest($"The user with {userId} does not exist.");
                    }
                }
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