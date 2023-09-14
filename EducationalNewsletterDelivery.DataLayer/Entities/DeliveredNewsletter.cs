using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalNewsletterDelivery.DataLayer.Entities
{
    public class DeliveredNewsletter
    {
        [Key]
        public int Id { get; set; }

        public string ReceiverID { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public bool IsSuccess { get; set; }

        //relations

        [Required]
        [ForeignKey(nameof(Newsletter))]
        public int NewsletterId { get; set; }
        
        public Newsletter Newsletter { get; set; }
    }
}