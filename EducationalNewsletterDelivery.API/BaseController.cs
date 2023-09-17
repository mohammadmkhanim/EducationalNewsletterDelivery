using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using EducationalNewsletterDelivery.DataLayer.Entities;

namespace EducationalNewsletterDelivery.API
{
    public abstract class BaseController<ControllerType> : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ILogger<ControllerType> _logger;
        protected readonly IConfiguration _configuration;
        protected readonly string _defaultBackendErrorMessage;
        protected int _userId
        {
            get
            {
                return Convert.ToInt32(User.FindFirstValue("id"));
            }
        }


        protected BaseController(
            IUnitOfWork unitOfWork = null,
            IMapper mapper = null,
            ILogger<ControllerType> logger = null,
            IConfiguration configuration = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        protected void LogError(Exception ex)
        {
            _logger.LogError($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] An error occurred{ex.StackTrace}: {ex.Message}");
        }

    }

}