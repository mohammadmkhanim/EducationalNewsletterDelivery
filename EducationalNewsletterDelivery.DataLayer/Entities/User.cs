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
        User, Admin, SuperAdmin
    }
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        
        public Role Role { get; set; }

        //relations
        public List<DeliveredNewsletter> DeliveredNewsletters { get; set; }
    }
}