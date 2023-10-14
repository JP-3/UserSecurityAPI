using System.Configuration;
using Funq;
using ServiceStack.Validation;
using UserSecurity.API.Interfaces.Managers;
using UserSecurity.API.Interfaces.Repositories;
using UserSecurity.API.Repositories.Database;
using UserSecurity.API.Repositories;
using UserSecurity.API.Managers;
using UserSecurity.API.Validation;
using ServiceStack;
using UserSecurity.API.Interfaces.Database;


namespace UserSecurity.API
{
    public static class ContainerManager
    {
        public static void Register(Container container)
        {
            // add service validation
            container.RegisterValidators(ReuseScope.Container, typeof(ValidationInfo).Assembly);

            //Managers
            container.RegisterAutoWiredAs<UserSecurityManager, IUserSecurityManager>();
            container.RegisterAutoWiredAs<PartyManager, IPartyManager>();

            //Repositories
            container.RegisterAutoWiredAs<ExpressRepository, IExpressRepository>();
            container.RegisterAutoWiredAs<PartyRepository, IPartyRepository>();
            container.RegisterAutoWiredAs<SqlConnection, ISqlConnection>();
            container.RegisterAutoWiredAs<SqlCommand, ISqlCommand>();
            container.RegisterAutoWiredAs<SqlDataReader, ISqlDataReader>();

            //serviceClients
            container.Register<IServiceClient>(new JsonServiceClient(ConfigurationManager.AppSettings["MDMEndpoint"]));
        }
    }
}
