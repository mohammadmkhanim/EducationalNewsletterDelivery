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
        private readonly EducationalNewsletterDeliveryDBContext _context;

        public UserRepository(EducationalNewsletterDeliveryDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<bool> ExisUserBytUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> ExistUserByIdAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task PromoteUserToAdminRoleAsync(int id)
        {
            var user = await GetByIdAsync(id);
            user.Role = Role.Admin;
            Update(user);
        }

        public async Task DemoteUserToUserRoleAsync(int id)
        {
            var user = await GetByIdAsync(id);
            user.Role = Role.User;
            Update(user);
        }
    }
}
