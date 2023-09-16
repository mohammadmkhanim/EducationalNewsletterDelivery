using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.Repository.GenericRepository;

namespace EducationalNewsletterDelivery.DataLayer.Repository.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<bool> ExisUserBytUsername(string username);
        public Task<bool> ExistUserById(int id);
        public Task<User?> GetUserByUsernameAndPassword(string username, string password);
    }
}