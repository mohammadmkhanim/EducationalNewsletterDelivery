using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EducationalNewsletterDelivery.API.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class DeliveryController : BaseController<DeliveryController>
    {
        // public async 

        [HttpGet]
        public string Get()
        {
            return "ss";
        }
    }
}