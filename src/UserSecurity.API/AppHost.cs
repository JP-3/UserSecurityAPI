using Funq;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Auth;
using ServiceStack.Text;
using ServiceStack.Validation;
using UserSecurity.API.ServiceDefinition;
using UserSecurity.API.Validation;

namespace UserSecurity.API
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost() : base("UserSecurity.API", typeof(ServiceDefinitionInfo).Assembly) { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            JsConfig.EmitCamelCaseNames = true;
            JsConfig.DateHandler = DateHandler.ISO8601;

            ContainerManager.Register(container);

            InitializePlugins();
            InitializeContainer(container);
        }

        private void InitializePlugins()
        {
            Plugins.Add(new ValidationFeature());
            Plugins.Add(new PostmanFeature());
            Plugins.Add(new SwaggerFeature());            
            EnableAuth();
            EnableCors();
        }

        private void InitializeContainer(Container container)
        {
            container.RegisterValidators(ReuseScope.Container, typeof(ValidationInfo).Assembly);
        }

        private void EnableAuth()
        {
            //RFC 4648 mentions other alphabets, such as the "URL and Filename safe" Base 64 Alphabet, 
            //where + and / got replaced with - and _.
            // swap - and _ with their Base64 string equivalents
            var secret = AppSettings.GetString("AuthSecret").Replace('-', '+').Replace('_', '/');
            var audience = AppSettings.GetString("AuthClientId");

            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] {
                        new JwtAuthProviderReader(AppSettings) {
                            HashAlgorithm = "HS256",
                            //PublicKeyXml = AppSettings.GetString("PublicKeyXml"),
                            AuthKeyBase64 = secret,
                            Audience = audience,
                            RequireSecureConnection = false
                        },
                }
            ));
        }

        /// <summary>
        /// Enables the cors.
        /// </summary>
        private void EnableCors()
        {
            Plugins.Add(new CorsFeature(allowedHeaders: "X-Requested-With, Content-Type, Authorization, Origin, Accept"));

            PreRequestFilters.Add((httpReq, httpRes) =>
            {
                //Handles Request and closes Responses after emitting global HTTP Headers
                if (httpReq.Verb == "OPTIONS")
                    httpRes.EndRequest(); //add a 'using ServiceStack;'
            });
        }



    }
}
