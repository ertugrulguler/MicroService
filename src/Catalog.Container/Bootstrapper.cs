using Autofac;
using Catalog.ApplicationService.Communicator.BackOffice;
using Catalog.ApplicationService.Communicator.Campaign;
using Catalog.ApplicationService.Communicator.Contract;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Parameter;
using Catalog.ApplicationService.Communicator.Search;
using Catalog.ApplicationService.Communicator.User;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Container.Modules;

using Framework.Core.Authorization;
using Framework.Core.Helpers.Abstract;
using Framework.Core.Helpers.Concrete;
using Framework.Core.Logging;

namespace Catalog.Container
{
    public class Bootstrapper
    {
        public static ILifetimeScope Container { get; private set; }

        public static void RegisterModules(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new MediatRModule());
            containerBuilder.RegisterModule(new RepositoryModule());
            containerBuilder.RegisterModule(new LoggingModule());
            containerBuilder.RegisterModule(new AssemblerModule());

            containerBuilder.RegisterType<CorrelationIdHelper>().As<ICorrelationIdHelper>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<UserCommunicator>().As<IUserCommunicator>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<BackOfficeCommunicator>().As<IBackOfficeCommunicator>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<MerhantCommunicator>().As<IMerhantCommunicator>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ParameterCommunicator>().As<IParameterCommunicator>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SearchCommunicator>().As<ISearchCommunicator>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ContractCommunicator>().As<IContractCommunicator>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CustomerHelper>().As<ICustomerHelper>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CampaignCommunicator>().As<ICampaignCommunicator>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<HttpRequestHelper>().As<IHttpRequestHelper>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DiscountService>().As<IDiscountService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<IdentityContext>().As<IIdentityContext>().InstancePerLifetimeScope();
        }

        public static void SetContainer(ILifetimeScope autofacContainer)
        {
            Container = autofacContainer;
        }
    }
}