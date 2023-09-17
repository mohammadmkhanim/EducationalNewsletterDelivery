using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalNewsletterDelivery.API.Models
{
    public class DeliveredNewsletterDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}