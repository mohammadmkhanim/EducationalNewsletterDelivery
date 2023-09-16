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
    public class NewsletterController : BaseController<NewsletterController>
    {

        public NewsletterController(IUnitOfWork unitOfWork,
                          IMapper mapper,
                          ILogger<NewsletterController> logger
                          ) : base(unitOfWork, mapper, logger)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsletterDTO>>> Get()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newsletterDTOs = await GetNewslettersAsync();
                return Ok(newsletterDTOs);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, _defaultBackendErrorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsletterDTO>> Get(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newsletter = await _unitOfWork.NewsletterRepository.GetByIdAsync(id);
                var newsletterDTO = _mapper.Map<NewsletterDTO>(newsletter);
                return Ok(newsletterDTO);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, _defaultBackendErrorMessage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsletterDTO createNewsletterDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Newsletter newsletter = _mapper.Map<Newsletter>(createNewsletterDTO);
                await _unitOfWork.NewsletterRepository.AddAsync(newsletter);
                await _unitOfWork.SaveAsync();
                var newsletterDTOs = await GetNewslettersAsync();
                return Ok(newsletterDTOs);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, _defaultBackendErrorMessage);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(NewsletterDTO newsletterDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Newsletter newsletter = _mapper.Map<Newsletter>(newsletterDTO);
                _unitOfWork.NewsletterRepository.Update(newsletter);
                await _unitOfWork.SaveAsync();
                var newsletterDTOs = await GetNewslettersAsync();
                return Ok(newsletterDTOs);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, _defaultBackendErrorMessage);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _unitOfWork.NewsletterRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                var newsletterDTOs = await GetNewslettersAsync();
                return Ok(newsletterDTOs);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, _defaultBackendErrorMessage);
            }
        }

        private async Task<IEnumerable<NewsletterDTO>> GetNewslettersAsync()
        {
            var newsletters = await _unitOfWork.NewsletterRepository.GetAsync();
            var newsletterDTOs = _mapper.Map<List<NewsletterDTO>>(newsletters);
            return newsletterDTOs;
        }
    }
}