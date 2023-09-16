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

        [Required]
        public DateTime DeliveredDateTime { get; set; }

        public DateTime? ReceivedDateTime { get; set; }

        public DateTime? SeenDateTime { get; set; }

        //relations
        [Required]
        [ForeignKey(nameof(Newsletter))]
        public int NewsletterId { get; set; }
        public Newsletter Newsletter { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}