using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.Repository.GenericRepository;

namespace EducationalNewsletterDelivery.DataLayer.Repository.IRepositories
{
    public interface IDeliveredNewsletterRepository : IGenericRepository<DeliveredNewsletter>
    {
        public Task MarkDeliveredNewsletterAsReceivedAsync(int id);
        public Task MarkDeliveredNewsletterAsSeenAsync(int id);
        public Task<bool> IsDeliveredNewsletterSeenAsync(int id);
        public Task<bool> IsDeliveredNewsletterReceivedAsync(int id);
    }
}