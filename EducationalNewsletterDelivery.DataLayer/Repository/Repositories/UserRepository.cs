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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private DbSet<User> _users;

        public UserRepository(EducationalNewsletterDeliveryDBContext context) : base(context)
        {
            _users = context.Users;
        }

        public Task<User?> GetUserByUsernameAndPassword(string username, string password)
        {
            return _users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public Task<bool> ExisUserBytUsername(string username)
        {
            return _users.AnyAsync(u => u.Username == username);
        }

        public Task<bool> ExistUserById(int id)
        {
            return _users.AnyAsync(u => u.Id == id);
        }
    }
}
