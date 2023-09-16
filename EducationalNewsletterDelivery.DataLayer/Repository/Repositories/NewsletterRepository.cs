using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalNewsletterDelivery.DataLayer.Context;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.Repository.GenericRepository;
using EducationalNewsletterDelivery.DataLayer.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EducationalNewsletterDelivery.DataLayer.Repository.Repositories
{
    public class NewsletterRepository : GenericRepository<Newsletter>, INewsletterRepository
    {
        private DbSet<Newsletter> _newsletters;

        public NewsletterRepository(EducationalNewsletterDeliveryDBContext context) : base(context)
        {
            _newsletters = context.Newsletters;
        }

        public Task<bool> ExistNewsletter(int id)
        {
            return _newsletters.AnyAsync(n => n.Id == id);
        }
    }
}