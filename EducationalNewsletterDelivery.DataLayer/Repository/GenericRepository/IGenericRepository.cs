﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationalNewsletterDelivery.DataLayer.Repository.GenericRepository
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        public Task<List<Entity>> GetAsync(
            Expression<Func<Entity, bool>> filter = null,
            Expression<Func<Entity, string>> orderBy = null,
            Expression<Func<Entity, string>> orderByDescending = null,
            List<string> includeProperties = null);

        public Task<Entity> GetByIdAsync(object id);

        public Task AddAsync(Entity entity);

        public Task AddAsync(List<Entity> entities);

        public Task DeleteAsync(object id);

        public void Delete(Entity entityToDelete);

        public void Update(Entity entityToUpdate);
    }
}
