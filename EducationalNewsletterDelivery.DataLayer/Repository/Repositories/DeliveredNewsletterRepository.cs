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
    public class DeliveredNewsletterRepository : GenericRepository<DeliveredNewsletter>, IDeliveredNewsletterRepository
    {
        private DbSet<DeliveredNewsletter> _deliveredNewsletters;

        public DeliveredNewsletterRepository(EducationalNewsletterDeliveryDBContext context) : base(context)
        {
            _deliveredNewsletters = context.DeliveredNewsletters;
        }
    }
}