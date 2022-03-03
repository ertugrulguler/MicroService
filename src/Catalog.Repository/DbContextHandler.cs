using Catalog.Domain;
using Framework.Core.Logging;
using System;
using System.Threading.Tasks;

namespace Catalog.Repository
{
    public class DbContextHandler : IDbContextHandler
    {
        private readonly CatalogDbContext _dbContext;
        private readonly IAppLogger _appLogger;

        public DbContextHandler(CatalogDbContext dbContext, IAppLogger appLogger)
        {
            _dbContext = dbContext;
            _appLogger = appLogger;
        }


        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _appLogger.Exception(ex, null);
                _dbContext.ChangeTracker.Clear();
                throw ex;
            }
        }
    }
}