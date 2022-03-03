using Catalog.Domain.Entities;
using EFCore.BulkExtensions;
using Framework.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.Domain
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> Table();
        Task<TEntity> GetByIdAsync(Guid id, bool isActive = true);
        Task<List<TEntity>> AllAsync(bool isActive = true);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate, bool isActive = true);
        Task<List<TEntity>> FilterEagerLoading(Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          string includeProperties = "", int? page = null, int? pageSize = null);
        Task SaveAsync(TEntity entity);
        //to do update has to be async
        TEntity Update(TEntity entity);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool isActive = true);
        Task<PagedList<TEntity>> FilterByPagingAsync(Expression<Func<TEntity, bool>> predicate,
            PagerInput pagerInput, bool isActive = true);
        void Delete(TEntity entity);
        Task InsertOrUpdateOrDeleteBulkExtension(List<TEntity> entityList);
        bool BulkMerge(List<TEntity> entityList);
        Task<List<TEntity>> BulkRead(List<TEntity> entityList, BulkConfig bulkConfig = null);
        Task<List<TEntity>> BulkInsert(List<TEntity> entityList);
        Task<bool> Exist(Expression<Func<TEntity, bool>> predicate);
    }
}