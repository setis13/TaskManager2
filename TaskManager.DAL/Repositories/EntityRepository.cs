using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TaskManager.DAL.Contracts.Context;
using TaskManager.DAL.Contracts.Entities.Base;
using TaskManager.DAL.Contracts.Repositories;

namespace TaskManager.DAL.Repositories {
    /// <summary>
    ///     Base entity repository implementation </summary>
    /// <typeparam name="T">BaseEntity</typeparam>
    public class EntityRepository<T> :  IEntityRepository<T> where T : BaseEntity {

        /// <summary>
        ///     Holds db context instance </summary>
        protected readonly DbContext Context;

        /// <summary>
        ///     Holds generic db set </summary>
        protected readonly DbSet<T> DbSet;

        /// <summary>
        ///     Creates entity repository </summary>
        /// <param name="dbContext">Db Context</param>
        public EntityRepository(ITaskManagerDbContext dbContext) {
            Context = dbContext.DbContext;
            DbSet = Context.Set<T>();
        }

        /// <summary>
        ///     Get entity by PK Id </summary>
        /// <remarks>CACHED VALUE</remarks>
        /// <param name="id">Id</param>
        /// <returns>Entity instance</returns>
        public virtual T GetById(params object[] id) {
            return DbSet.Find(id);
        }

        /// <summary>
        ///     Gets all entities list </summary>
        /// <returns>Entities list</returns>
        public IQueryable<T> GetAll() {
            return DbSet.Where(entity => !entity.IsDeleted);
        }

        /// <summary>
        ///     Search entities using predicate expression </summary>
        /// <param name="predicate">Predicate expression</param>
        /// <returns>Entities list</returns>
        public virtual IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate) {
            return GetAll().Where(predicate);
        }

        /// <summary>
        ///     Insert a new entity </summary>
        /// <param name="entity">Entity instance</param>
        public void Insert(T entity) {
            entity.CreatedDate = entity.LastModifiedDate = DateTime.UtcNow;
            var entityEntry = Context.Entry(entity);
            if (entityEntry.State != EntityState.Detached) {
                entityEntry.State = EntityState.Added;
            } else {
                DbSet.Add(entity);
            }
        }

        /// <summary>
        ///     Update existing entity </summary>
        /// <param name="entity">Entity instance</param>
        public void Update(T entity) {
            entity.LastModifiedDate = DateTime.UtcNow;
            var entityEntry = Context.Entry(entity);
            if (entityEntry.State == EntityState.Detached) {
                DbSet.Attach(entity);
            }
            entityEntry.State = EntityState.Modified;
        }

        /// <summary>
        ///     Delete existing entity by its ids </summary>
        /// <param name="ids">List of Entity Ids</param>
        public void MarkAsDeletedIds(List<Guid> ids) {
            var entites = SearchFor(p => ids.Contains(p.EntityId));
            foreach (var entity in entites) {
                MarkAsDeleted(entity);
            }
        }

        /// <summary>
        ///     Delete existing entity bby its id </summary>
        /// <param name="id">Entity Id</param>
        public virtual void MarkAsDeletedId(params object[] id) {
            var entity = GetById(id);
            if (entity == null) return;
            MarkAsDeleted(entity);
        }

        /// <summary>
        ///     Delete existing entity </summary>
        /// <param name="entity">Entity instance</param>
        public void MarkAsDeleted(T entity) {
            entity.IsDeleted = true;
        }
    }
}