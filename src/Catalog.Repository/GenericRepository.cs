using Catalog.Domain;
using Catalog.Domain.Entities;


using EFCore.BulkExtensions;
using Framework.Core.Model;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.Repository
{

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbContext _dbContext;
        public readonly DbSet<TEntity> _entities;

        protected GenericRepository(DbContext context)
        {
            _entities = context.Set<TEntity>();
            _dbContext = context;
        }

        public IQueryable<TEntity> Table()
        {
            return _entities.AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync(Guid id, bool isActive = true)
        {
            return await _entities.SingleOrDefaultAsync(s => s.Id == id && s.IsActive == isActive);
        }

        public async Task<List<TEntity>> AllAsync(bool isActive = true)
        {
            return await _entities.Where(s => s.IsActive == isActive).AsQueryable().ToListAsync();
        }
        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.SingleOrDefaultAsync(predicate);
        }

        public async Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate, bool isActive = true)
        {
            return await _entities.AsNoTracking().Where(predicate).Where(s => s.IsActive == isActive).ToListAsync();
        }

        public async Task<List<TEntity>> FilterEagerLoading(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "", int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _entities.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return await query.ToListAsync();
        }

        public async Task SaveAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public TEntity Update(TEntity entity)
        {
            var entityEntry = _entities.Update(entity);
            return entityEntry.Entity;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool isActive = true)
        {
            return await _entities.Where(predicate).Where(s => s.IsActive == isActive).CountAsync();
        }

        public async Task<PagedList<TEntity>> FilterByPagingAsync(Expression<Func<TEntity, bool>> predicate,
            PagerInput pagerInput, bool isActive = true)
        {
            var itemsCount = await CountAsync(predicate);

            var list = await _entities.Where(predicate).Where(s => s.IsActive == isActive).OrderByDescending(c => c.CreatedDate)
                .Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize)
                .Take(pagerInput.PageSize)
                .ToListAsync();

            return new PagedList<TEntity>(list, itemsCount, pagerInput);
        }

        public void Delete(TEntity entity)
        {
            entity.setIsActive(false);
        }


        public async Task InsertOrUpdateOrDeleteBulkExtension(List<TEntity> entityList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.BulkInsertOrUpdateOrDeleteAsync(entityList);
                transaction.Commit();
            }
        }

        public async Task<List<TEntity>> BulkRead(List<TEntity> entityList, BulkConfig bulkConfig = null)
        {
            //var items = itemsNames.Select(a => new Item { Name = a }); // creating list of Items where only Name is set
            //var bulkConfig = new BulkConfig { UpdateByProperties = new List<string> { nameof(Item.Name) } };
            await _dbContext.BulkReadAsync(entityList, bulkConfig); // Items list will be loaded from Db with data(other properties)
            return entityList;
        }

        public async Task<List<TEntity>> BulkInsert(List<TEntity> entityList)
        {
            entityList.ForEach(x => { x.setIsActive(true); x.CreatedDate = DateTime.Now; });
            await _dbContext.BulkInsertAsync(entityList);
            return entityList;
        }
        public bool BulkMerge(List<TEntity> entityList)
        {
            var value = false;
            var connection = _dbContext.Database.CreateExecutionStrategy();
            connection.Execute(
                () =>
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.BulkInsertOrUpdate(entityList);
                            transaction.Commit();
                            value = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            transaction.Rollback();
                            value = false;
                        }
                    }
                });
            return value;
        }

        public async Task<bool> Exist(Expression<Func<TEntity, bool>> predicate)
        {
            var exist = _entities.Where(predicate);
            return await exist.AnyAsync();
        }
    }
}