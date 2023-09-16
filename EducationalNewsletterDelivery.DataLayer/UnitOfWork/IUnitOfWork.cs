using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalNewsletterDelivery.DataLayer.Repository.IRepositories;

namespace EducationalNewsletterDelivery.DataLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IDeliveredNewsletterRepository DeliveredNewsletterRepository { get; }
        public INewsletterRepository NewsletterRepository { get; }
        public IUserRepository UserRepository { get; }
        public Task SaveAsync();
    }
}
