using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace OwinPoc
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var container = new Container();
            //var controllerActivator = new MyControllerActivator(container);

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register<IUserRepository, InMemoryUserRepository>(Lifestyle.Scoped);

            container.Register(() => container.IsVerifying ? WindowsIdentity.GetCurrent() : Lifestyle.Scoped.GetCurrentScope(container).Resolve<IIdentity>(), Lifestyle.Scoped);
            container.Register(() => container.IsVerifying ? ScopeId.Empty : Lifestyle.Scoped.GetCurrentScope(container).Resolve<ScopeId>(), Lifestyle.Scoped);

            container.RegisterWebApiControllers(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            //config.Services.Replace(
            //    typeof(IHttpControllerActivator),
            //    controllerActivator);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var listener = (HttpListener)app.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;

            app.Use(async (ctx, next) =>
            {
                Console.WriteLine($"Log: url => {ctx.Request.Uri}");
                
                var identity = ctx.Authentication?.User?.Identity;
                
                using (var scope = AsyncScopedLifestyle.BeginScope(container))
                {
                    scope
                        .AddInstance(identity)
                        .AddInstance(ctx)
                        .AddInstance(ScopeId.NewId());

                    await next();
                }
            });

            app.UseWebApi(config);
        }
    }
}