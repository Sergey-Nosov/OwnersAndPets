using System.Configuration;
using Autofac;
using OwnersAndPets.Data;

namespace OwnersAndPets.Autofac
{
    public class Module : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            builder.RegisterType(typeof(Context)).WithParameter("connectionString", connectionString).InstancePerRequest();
        }
    }
}