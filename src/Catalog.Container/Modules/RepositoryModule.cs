using Autofac;
using Catalog.Repository;
using Catalog.Repository.RepositoryAggregate.CategoryRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Module = Autofac.Module;

namespace Catalog.Container.Modules
{
    public class RepositoryModule : Module
    {
        private static string _connectionString;

        public static void AddDbContext(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _connectionString = configuration["DbConnString"];
            //todo
            serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<CatalogDbContext>(options => options.UseSqlServer(_connectionString));
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assemblyType = typeof(CategoryRepository).GetTypeInfo();

            builder.RegisterAssemblyTypes(assemblyType.Assembly)
                .Where(x => x != typeof(CatalogDbContext))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}