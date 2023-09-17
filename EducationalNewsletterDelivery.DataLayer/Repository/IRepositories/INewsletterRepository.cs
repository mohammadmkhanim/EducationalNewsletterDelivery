using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.Repository.GenericRepository;

namespace EducationalNewsletterDelivery.DataLayer.Repository.IRepositories
{
    public interface INewsletterRepository : IGenericRepository<Newsletter>
    {
        public Task<bool> ExistNewsletterByIdAsync(int id);
        public Task<List<Newsletter>> GetUserNewslettersAsync(int userId);
    }
}