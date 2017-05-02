using System.Reflection;
using System.Web.Mvc;
using Autofac;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Module = OwnersAndPets.Autofac.Module;

namespace OwnersAndPets
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterControllers(GetType().Assembly);
            containerBuilder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterFilterProvider();
            containerBuilder.RegisterSource(new ViewRegistrationSource());
            containerBuilder.RegisterModule(new Module());
            IContainer container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
