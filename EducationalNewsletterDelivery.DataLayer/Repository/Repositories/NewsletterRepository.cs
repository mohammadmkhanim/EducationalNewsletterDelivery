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
        private readonly EducationalNewsletterDeliveryDBContext _context;

        public NewsletterRepository(EducationalNewsletterDeliveryDBContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> ExistNewsletterByIdAsync(int id)
        {
            return _context.Newsletters.AnyAsync(n => n.Id == id);
        }

        public async Task<List<Newsletter>> GetUserNewslettersAsync(int userId)
        {
            var userNewsletters = await _context.DeliveredNewsletters.Where(d => d.UserId == userId).Select(d => d.Newsletter).ToListAsync();
            return userNewsletters;
        }
    }
}