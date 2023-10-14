using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Testing;
using ServiceStack.Text;

namespace UserSecurity.API.Tests.UnitTests
{
    [TestClass]
    public class AppHostShould
    {
        [TestMethod]
        public void Configure_AllScenarios_JsonConfigured(){
           
            //Arrange
            var host = new BasicAppHost
            {
                TestMode = true,
                ConfigureContainer = container =>
                {
                    container.Register<IAuthSession>(c => new AuthUserSession
                    {
                        UserName = "Mocked",
                    });
                }

            }.Init();

            var appHost = new AppHost();
            var funqContainer = new Funq.Container();

            //Act
            appHost.Configure(funqContainer);

            //Assert
            Assert.IsTrue(JsConfig.EmitCamelCaseNames);
            Assert.AreEqual(DateHandler.ISO8601, JsConfig.DateHandler);
            host.Dispose();
        }
     
    }
}
