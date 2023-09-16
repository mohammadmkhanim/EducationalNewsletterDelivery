using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalNewsletterDelivery.API.Models
{
    public class UserDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        //relations
        // public List<DeliveredNewsletter> DeliveredNewsletters { get; set; }
    }

    public class AuthUserDTO
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}