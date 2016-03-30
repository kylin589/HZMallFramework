using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.ResolveAnything;
using HZMall.IServices.Products;

namespace HZMall.IServices
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Configure Autofac to create a new instance of any type that implements ICommand when such type is requested

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<ICategoryService>()));
        }

    }
}
