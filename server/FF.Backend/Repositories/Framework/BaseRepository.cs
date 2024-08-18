using FF.Backend.Contexts;
using FF.Backend.Domain;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FF.Backend.Repositories.Framework
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {

        protected readonly FFContext Database;

        protected BaseRepository(FFContext database)
        {
            Database = database;
        }

        public T GetById(object id)
        {
            return Database.Set<T>().Find(id);
        }

        public IQueryable<T> Get()
        {
            return Database.Set<T>();
        }

        public virtual T Insert(T entity)
        {
            Database.Set<T>().Add(entity);
            return entity;
        }

        public virtual T Update(T entity)
        {
            Database.Set<T>().Attach(entity);
            Database.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public virtual T Update(T entity, int id)
        {
            var entry = Database.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var attachedEntity = Database.Set<T>().Find(id);

                if (attachedEntity != null)
                {
                    entity.Id = attachedEntity.Id;

                    var attachedEntry = Database.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
            return entity;
        }

        public virtual void SoftDelete(object id)
        {
            var entity = GetById(id);

            if (entity == null)
                throw new Exception("This object does not exists");

            Update(entity);
        }

        public virtual void Delete(T entity)
        {
            Database.Set<T>().Remove(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = GetById(id);
            Delete(entity);

        }
    }
}
