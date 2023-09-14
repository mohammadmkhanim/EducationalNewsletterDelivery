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
        private DbSet<Newsletter> _newsletter;

        public NewsletterRepository(EducationalNewsletterDeliveryDBContext context) : base(context)
        {
            _newsletter = context.Newsletters;
        }
    }
}