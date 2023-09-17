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
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> SendNewsletterToUsersAsync(SendNewsletterDTO sendNewsletterDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (!await _unitOfWork.NewsletterRepository.ExistNewsletterByIdAsync(sendNewsletterDTO.NewsletterId))
                {
                    return BadRequest("The newsletter does not exist.");
                }
                foreach (var userId in sendNewsletterDTO.UserIds)
                {
                    if (!await _unitOfWork.UserRepository.ExistUserByIdAsync(userId))
                    {
                        return BadRequest($"The user with {userId} does not exist.");
                    }
                    DeliveredNewsletter deliveredNewsletter = new DeliveredNewsletter()
                    {
                        DeliveredDateTime = DateTime.Now,
                        NewsletterId = sendNewsletterDTO.NewsletterId,
                        UserId = userId
                    };
                    await _unitOfWork.DeliveredNewsletterRepository.AddAsync(deliveredNewsletter);
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

        [HttpPost]
        public async Task<IActionResult> GetUserNewslettersAsync()
        {
            try
            {
                var deliveredNewsletters = await _unitOfWork.DeliveredNewsletterRepository.GetAsync(d => d.UserId == _userId);
                foreach (var deliveredNewsletter in deliveredNewsletters)
                {
                    if (!await _unitOfWork.DeliveredNewsletterRepository.IsDeliveredNewsletterReceivedAsync(deliveredNewsletter.Id))
                    {
                        await _unitOfWork.DeliveredNewsletterRepository.MarkDeliveredNewsletterAsReceivedAsync(deliveredNewsletter.Id);
                    }
                    if (!await _unitOfWork.DeliveredNewsletterRepository.IsDeliveredNewsletterSeenAsync(deliveredNewsletter.Id))
                    {
                        await _unitOfWork.DeliveredNewsletterRepository.MarkDeliveredNewsletterAsSeenAsync(deliveredNewsletter.Id);
                    }
                }
                await _unitOfWork.SaveAsync();
                var userNewsletters = await _unitOfWork.NewsletterRepository.GetUserNewslettersAsync(_userId);
                var userNewsletterDTOs = _mapper.Map<List<NewsletterDTO>>(userNewsletters);
                return Ok(userNewsletterDTOs);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, _defaultBackendErrorMessage);
            }
        }
    }
}