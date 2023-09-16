using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EducationalNewsletterDelivery.DataLayer.Entities
{
    public enum Role
    {
        User, Admin
    }
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        
        public Role Role { get; set; }

        //relations
        public List<DeliveredNewsletter> DeliveredNewsletters { get; set; }
    }
}