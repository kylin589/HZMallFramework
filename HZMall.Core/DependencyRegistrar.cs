using Autofac;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using HZMall.Framework.Dapper;
using HZMall.Core.Data;
using HZMall.Core.Security;

namespace HZMall.Core
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 0; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.Register<IBcDbContext>(c => new BcDbContext("name=BC"))
                .InstancePerLifetimeScope();

            builder.Register<IDatabaseHelper>(c => new HkDataBaseHelper("BC"))
                .InstancePerLifetimeScope();

            builder.Register(c => new DataUtils("BC"))
                .InstancePerLifetimeScope();

            builder.RegisterType<SecuritySettings>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<MemCacheFactory>().As<ICacheManager>().InstancePerLifetimeScope();
        }
    }
}
