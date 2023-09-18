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
        public Task<bool> ExisUserByUsernameAsync(string username);
        public Task<bool> ExistUserByIdAsync(int id);
        public Task PromoteUserToAdminRoleAsync(int id);
        public Task DemoteUserToUserRoleAsync(int id);
        public Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password);
    }
}