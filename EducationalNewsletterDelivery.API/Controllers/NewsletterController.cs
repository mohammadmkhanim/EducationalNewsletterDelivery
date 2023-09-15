using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EducationalNewsletterDelivery.API.Models;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.Repository.IRepositories;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace EducationalNewsletterDelivery.API.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class NewsletterController : BaseController<NewsletterController>
    {
        INewsletterRepository _newsletterRepository;

        public NewsletterController(IUnitOfWork unitOfWork,
                          IMapper mapper,
                          ILogger<NewsletterController> logger
                          ) : base(unitOfWork, mapper, logger)
        {
            _newsletterRepository = _unitOfWork.NewsletterRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Newsletter>>> Get()
        {
            var Newsletters = await _newsletterRepository.GetAsync();
            return Ok(Newsletters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult< IEnumerable<Newsletter>>> Get(int id)
        {
            var newsletter = await _newsletterRepository.GetByIdAsync(id);
            var newsletterDTO = _mapper.Map<NewsletterDTO>(newsletter);
            return Ok(newsletterDTO);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsletterDTO createNewsletterDTO)
        {
            Newsletter newsletter = _mapper.Map<Newsletter>(createNewsletterDTO);
            await _newsletterRepository.AddAsync(newsletter);
            await _unitOfWork.SaveAsync();
            return Ok(newsletter);
        }

        [HttpPut]
        public async Task<IActionResult> Update(NewsletterDTO newsletterDTO)
        {
            Newsletter newsletter = _mapper.Map<Newsletter>(newsletterDTO);
            _newsletterRepository.Update(newsletter);
            await _unitOfWork.SaveAsync();

            var newsletters = await _newsletterRepository.GetAsync();
            var newsletterDTOs = _mapper.Map<List<Newsletter>>(newsletters);
            return Ok(newsletterDTOs);
        }

    }
}