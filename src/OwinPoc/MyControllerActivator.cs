using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using SimpleInjector;

namespace OwinPoc
{
    public class MyControllerActivator : IHttpControllerActivator
    {
        private readonly Container container;
        private readonly IHttpControllerActivator inner;

        public MyControllerActivator(Container container)
        {
            this.container = container;
            inner = new DefaultHttpControllerActivator();
        }
        
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var constructorInfo = controllerType.GetConstructors().FirstOrDefault();
            if (constructorInfo == null)
                return inner.Create(request, controllerDescriptor, controllerType);

            
            var owinContext = request.GetOwinContext();
            var identity = owinContext.Authentication?.User?.Identity;
            var scope = Lifestyle.Scoped.GetCurrentScope(container);
            scope?.SetItem("identity", identity);
            scope?.SetItem("owinContext", owinContext);

            var parameterInfos = constructorInfo.GetParameters();
            var services = parameterInfos.Select(p => Resolve(request, p.ParameterType)).ToArray();

            return (IHttpController)constructorInfo.Invoke(services);
        }

        private object Resolve(HttpRequestMessage request, Type parameterType)
        {
            var owinContext = request.GetOwinContext();
            var identity = owinContext.Authentication?.User?.Identity;

            if (parameterType == typeof(IOwinContext))
                return owinContext;

            if (parameterType == typeof(IIdentity))
                return identity;

            var scope = request.GetDependencyScope();

            return scope.GetService(parameterType);
        }
    }
}