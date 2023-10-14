using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;
using UserSecurity.API.Interfaces.Managers;
using UserSecurity.API.Interfaces.Repositories;
using UserSecurity.API.Managers;
using UserSecurity.API.Repositories;

namespace UserSecurity.API.Tests.UnitTests
{
    [TestClass]
    public class ContainerManagerShould
    {
        [TestMethod]
        public void Register_AllScenario_AllTypes()
        {
            //Arrange
            var funqContainer = new Funq.Container();

            //Act
            ContainerManager.Register(funqContainer);

            //Assert
            //Managers
            Assert.AreEqual(typeof(UserSecurityManager), funqContainer.Resolve<IUserSecurityManager>().GetType());
            Assert.AreEqual(typeof(PartyManager), funqContainer.Resolve<IPartyManager>().GetType());

            //Respositories
            Assert.AreEqual(typeof(ExpressRepository), funqContainer.Resolve<IExpressRepository>().GetType());
            Assert.AreEqual(typeof(PartyRepository), funqContainer.Resolve<IPartyRepository>().GetType());

            //Clients
            Assert.AreEqual(typeof(JsonServiceClient), funqContainer.Resolve<IServiceClient>().GetType());
        }
    }
}
