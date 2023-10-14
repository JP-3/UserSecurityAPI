using System;
using System.Diagnostics.CodeAnalysis;
using ServiceStack;
using Moq;
using UserSecurity.API.Interfaces.Repositories;
using UserSecurity.API.Repositories;
using PartyV5.ServiceModel.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserSecurity.API.Tests.UnitTests.Repositories
{
    [TestClass]
    public class PartyRepositoryShould
    {
        #region Properties

        private IPartyRepository PartyRepository { get; set; }

        private Mock<IServiceClient> MockJsonServiceClient { get; set; }

        #endregion

        #region Setup

        [TestInitialize]
        public void TestInitialize()
        {
            MockJsonServiceClient = new Mock<IServiceClient>();
            PartyRepository = new PartyRepository(MockJsonServiceClient.Object);

        }

        #endregion

        #region Public Methods       

        [TestMethod]
        public void GetBranch_AllScenario_CallsPartyRepository()
        {
            //Arrange
            var mockClient = new Mock<IServiceClient>();
            
            MockJsonServiceClient.Setup(client => client.Get<ReadBranchResponse>(It.IsAny<object>())).Returns(new ReadBranchResponse());

            ////Act
            var result = PartyRepository.GetBranch(string.Empty);

            //Assert
            MockJsonServiceClient.Verify(client => client.Get<ReadBranchResponse>(It.IsAny<object>()),Times.Once);
        }

        #endregion
    }
}
