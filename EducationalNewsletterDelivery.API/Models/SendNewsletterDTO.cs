using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalNewsletterDelivery.API.Models
{
    public class SendNewsletterDTO
    {
        [Required]
        public int NewsletterId { get; set; }

        [Required]
        public int[] UserIds { get; set; }
    }
}