using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalNewsletterDelivery.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace EducationalNewsletterDelivery.DataLayer.Context
{
    public class EducationalNewsletterDeliveryDBContext : DbContext
    {
        public EducationalNewsletterDeliveryDBContext(DbContextOptions<EducationalNewsletterDeliveryDBContext> options) : base(options)
        {
        }

        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<DeliveredNewsletter> DeliveredNewsletters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}