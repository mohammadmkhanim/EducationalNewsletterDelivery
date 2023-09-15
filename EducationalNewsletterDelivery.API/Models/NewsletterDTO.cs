using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalNewsletterDelivery.API.Models
{
    public class NewsletterDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }

    public class CreateNewsletterDTO
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}