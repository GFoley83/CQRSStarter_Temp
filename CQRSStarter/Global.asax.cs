using CQRSStarter.Commands.Contract;
using CQRSStarter.Commands.Implementation.Decorators;
using CQRSStarter.DAL;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Linq;
using System.Web.Http;
using Turnover.Command.Implementation.Decorators;

namespace CQRSStarter
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            // Create the container as usual.
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var assemblies = new[] { typeof(ICommandHandler<>).Assembly };

            // Register your types, for instance using the scoped lifestyle:
            container.Register<IRepository, CQRSStarterContext>(Lifestyle.Scoped);

            var types = container.GetTypesToRegister(typeof(ICommandHandler<>),
              assemblies,
              new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true })
              .ToList();

            container.Register(typeof(ICommandHandler<>), types.Where(t => !t.IsGenericTypeDefinition));

            foreach(var type in types.Where(t => t.IsGenericTypeDefinition))
            {
                container.Register(typeof(ICommandHandler<>), type);
            }

            // container.Register(typeof(ICommandHandler<>), assemblies);

            // Decorate each returned ICommandHandler<T> object with
            // a TransactionCommandHandlerDecorator<T>.
            container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(TransactionCommandHandlerDecorator<>));

            // Decorate each returned ICommandHandler<T> object with
            // a DeadlockRetryCommandHandlerDecorator<T>.
            container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(DeadlockRetryCommandHandlerDecorator<>));

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
              new SimpleInjectorWebApiDependencyResolver(container);


            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
