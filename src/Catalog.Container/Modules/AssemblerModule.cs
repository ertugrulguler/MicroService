using Autofac;

namespace Catalog.Container.Modules
{
    public class AssemblerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Catalog.ApplicationService"))
              .Where(t => t.Name.EndsWith("Assembler"))
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
