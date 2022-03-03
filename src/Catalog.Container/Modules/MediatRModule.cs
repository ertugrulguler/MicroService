using Autofac;
using AutoMapper;
using Catalog.ApiContract.Request.Query;
using Catalog.ApplicationService.AutoMapper;
using Catalog.ApplicationService.Handler.Query;
using Catalog.Container.Decorator;
using Catalog.Domain.Helpers;
using MediatR;
using System.Reflection;
using Module = Autofac.Module;

namespace Catalog.Container.Modules
{
    public class MediatRModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(typeof(HealthCheckQuery).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(HealthCheckQueryHandler).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder.RegisterGeneric(typeof(LoggingHandler<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ExceptionHandler<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(CacheHandler<,>)).As(typeof(IPipelineBehavior<,>));

            builder.RegisterType(typeof(Mapper)).As(typeof(IMapper)).AsSelf().InstancePerLifetimeScope();
            builder.RegisterType(typeof(AutoMapperConfiguration)).As(typeof(IAutoMapperConfiguration)).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType(typeof(ExpressionBinding)).As(typeof(IExpressionBinding)).InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}