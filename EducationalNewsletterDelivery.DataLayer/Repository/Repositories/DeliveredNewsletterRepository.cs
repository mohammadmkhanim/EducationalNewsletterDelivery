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
        private readonly EducationalNewsletterDeliveryDBContext _context;

        public DeliveredNewsletterRepository(EducationalNewsletterDeliveryDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsDeliveredNewsletterReceivedAsync(int id)
        {
            var deliveredNewsletter = await GetByIdAsync(id);
            return deliveredNewsletter.ReceivedDateTime != null;
        }

        public async Task<bool> IsDeliveredNewsletterSeenAsync(int id)
        {
            var deliveredNewsletter = await GetByIdAsync(id);
            return deliveredNewsletter.SeenDateTime != null;
        }

        public async Task MarkDeliveredNewsletterAsReceivedAsync(int id)
        {
            var deliveredNewsletter = await GetByIdAsync(id);
            deliveredNewsletter.ReceivedDateTime = DateTime.Now;
            Update(deliveredNewsletter);
        }

        public async Task MarkDeliveredNewsletterAsSeenAsync(int id)
        {
            var deliveredNewsletter = await GetByIdAsync(id);
            deliveredNewsletter.SeenDateTime = DateTime.Now;
            Update(deliveredNewsletter);
        }

        public async Task MarkNewslettersAsReceivedForUserAsync(int userId)
        {
            var deliveredNewsletters = await GetAsync(n => n.UserId == userId);
            foreach (var deliveredNewsletter in deliveredNewsletters)
            {
                deliveredNewsletter.ReceivedDateTime = DateTime.Now;
                Update(deliveredNewsletter);
            }
        }
    }
}