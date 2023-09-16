using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalNewsletterDelivery.DataLayer.Context;
using EducationalNewsletterDelivery.DataLayer.Repository.IRepositories;
using EducationalNewsletterDelivery.DataLayer.Repository.Repositories;

namespace EducationalNewsletterDelivery.DataLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private EducationalNewsletterDeliveryDBContext _context;
        public IDeliveredNewsletterRepository _deliveredNewsletterRepository;
        public INewsletterRepository _newsletterRepository;
        public IUserRepository _userRepository;

        public UnitOfWork(EducationalNewsletterDeliveryDBContext context)
        {
            _context = context;
        }

        public IDeliveredNewsletterRepository DeliveredNewsletterRepository
        {
            get
            {
                if (_deliveredNewsletterRepository == null)
                {
                    _deliveredNewsletterRepository = new DeliveredNewsletterRepository(_context);
                }
                return _deliveredNewsletterRepository;
            }
        }
        public INewsletterRepository NewsletterRepository
        {
            get
            {
                if (_newsletterRepository == null)
                {
                    _newsletterRepository = new NewsletterRepository(_context);
                }
                return _newsletterRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}