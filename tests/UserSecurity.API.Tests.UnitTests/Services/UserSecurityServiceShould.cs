using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserSecurity.API.Interfaces.Managers;
using UserSecurity.API.ServiceDefinition;
using Moq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Testing;
using UserSecurity.API.ServiceModel.Messages;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.Tests.UnitTests.Services
{
    /// <summary>
    /// Summary description for UserSecurityServiceShould
    /// </summary>
    [TestClass]
    public class UserSecurityServiceShould
    {


        #region Properties

        private UserSecurityService UserSecurityService { get; set; }

        private Mock<IUserSecurityManager> MockUserSecurtiyManager { get; set; }

        #endregion

        #region Setup

        [TestInitialize]
        public void TestInitialize()
        {
            MockUserSecurtiyManager = new Mock<IUserSecurityManager>();
            UserSecurityService = new UserSecurityService(MockUserSecurtiyManager.Object) { Request = new MockHttpRequest() };
        }

        #endregion

        #region Public Methods

        [TestMethod]
        public void GetSplitScreenSecurity_AllScenario_ReturnsGetSplitScreenSecurityResponse()
        {
            //Arrange
            //var host = new BasicAppHost
            //{
            //    TestMode = true,
            //    ConfigureContainer = container =>
            //    {
            //        container.Register<IAuthSession>(c => new AuthUserSession
            //        {
            //            UserName = "Mocked",
            //        });
            //    },

            //}.Init();
            MockUserSecurtiyManager.Setup(manager => manager.SplitScreenSecurity(It.IsAny<string>()))
                .Returns(new SplitScreenSecurity()
                {
                    StandardScreenSecurity = new List<ScreenSecurity>() {
                    new ScreenSecurity() { AccessLevel = "20", Default = 15, Id = 9999, Name = "TEST_USER", Sponsor = "TEST_SPONSOR",
                        SponsorScreenApproval = 25, SecurityLevels = new List<SecurityLevel>() } },

                    SponsorScreenSecurity = new List<ScreenSecurity>() {
                    new ScreenSecurity() { AccessLevel = "30", Default = 20, Id = 9999, Name = "TEST_USER", Sponsor = "TEST_SPONSOR",
                        SponsorScreenApproval = 50, SecurityLevels = new List<SecurityLevel>() } }
                });

            ////Act
            var response = UserSecurityService.Get(new GetSplitScreenSecurity() { UserName = "TEST_USER" });

            ////Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.SplitScreenSecurity.StandardScreenSecurity.Any());
            Assert.IsTrue(response.SplitScreenSecurity.SponsorScreenSecurity.Any());
            Assert.IsTrue(response.SplitScreenSecurity.StandardScreenSecurity.First().AccessLevel == "20");
            Assert.IsTrue(response.SplitScreenSecurity.SponsorScreenSecurity.Last().AccessLevel == "30");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.SplitScreenSecurity(It.IsAny<string>()), Times.Once);
        }


        [TestMethod]
        public void GetCompareSplitScreenSecurity_AllScenario_ReturnsGetSplitScreenSecurityResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.SplitCompareSecurity(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SplitCompareSecurity()
                {
                    StandardScreenSecurity = new List<CompareSecurity>() {
                    new CompareSecurity() { AccessLevel = "20", Default = 15, Id = 9999, Name = "TEST_USER", Sponsor = "TEST_SPONSOR",
                        SponsorScreenApproval = 25, SecurityLevels = new List<SecurityLevel>() } },

                    SponsorScreenSecurity = new List<CompareSecurity>() {
                    new CompareSecurity() { AccessLevel = "30", Default = 20, Id = 9999, Name = "TEST_USER", Sponsor = "TEST_SPONSOR",
                        SponsorScreenApproval = 50, SecurityLevels = new List<SecurityLevel>() } }
                });

            ////Act
            var response = UserSecurityService.Get(new GetCompareSplitScreenSecurity() { UserNameA = "TEST_USERA", UserNameB = "TEST_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.SplitCompareSecurity.StandardScreenSecurity.Any());
            Assert.IsTrue(response.SplitCompareSecurity.SponsorScreenSecurity.Any());
            Assert.IsTrue(response.SplitCompareSecurity.StandardScreenSecurity.First().AccessLevel == "20");
            Assert.IsTrue(response.SplitCompareSecurity.SponsorScreenSecurity.Last().AccessLevel == "30");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.SplitCompareSecurity(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetScreenSecurity_AllScenario_ReturnsGetScreenSecurityResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.ReadUserSecurity(It.IsAny<string>()))
                .Returns(new List<ScreenSecurity>() { new ScreenSecurity() { AccessLevel = "20", Default = 15, Id = 9999, Name = "TEST_USER", Sponsor = "TEST_SPONSOR", SponsorScreenApproval = 50, SecurityLevels = new List<SecurityLevel>() } });

            //Act
            var response = UserSecurityService.Get(new GetScreenSecurity() { UserName = "TEST_USER" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.ScreenSecurities.Any());
            Assert.IsTrue(response.ScreenSecurities.First().AccessLevel == "20");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.ReadUserSecurity(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void PostUserCertification_AllScenario_ReturnsUserCertificationResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.GrantAllUserCerts(It.IsAny<string>()))
                .Returns(new List<UserCertification>() { new UserCertification() { EmpTrainingID = 1, EmpCode = "TEST", CourseID = 1, DatePassed = DateTime.Today, ExpirationDate = DateTime.Today, EnteredDate = DateTime.Today, EnteredBy = "Security" } });

            //Act
            var response = UserSecurityService.Post(new PostGrantAllUserCerts() { UserName = "TEST_USER" });

            //Assert


            Assert.IsNotNull(response);
            Assert.IsTrue(response.UserCertificationList.First().EmpTrainingID == 1);
            Assert.IsTrue(response.UserCertificationList.First().EmpCode == "TEST");
            Assert.IsTrue(response.UserCertificationList.First().CourseID == 1);
            Assert.IsTrue(response.UserCertificationList.First().DatePassed == DateTime.Today);
            Assert.IsTrue(response.UserCertificationList.First().ExpirationDate == DateTime.Today);
            Assert.IsTrue(response.UserCertificationList.First().EnteredDate == DateTime.Today);
            Assert.IsTrue(response.UserCertificationList.First().EnteredBy == "Security");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.GrantAllUserCerts(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ReadEmployee_AllScenario_ReturnsEmployeeResponse()
        {
            const string BRANCH = "-6666";
            const string DEPARTMENT = "TEST_DEPARTMENT";
            const string MANAGER = "TEST_MANAGER";

            List<string> validEmployees = new List<string>() { "VALID_EMPLOYEE" };
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.ReadEmployee(It.IsAny<string>()))
                .Returns(new EmployeeInfo() { Branch = BRANCH, Department = DEPARTMENT, Manager = MANAGER });

            //Act
            var response = UserSecurityService.Get(new GetEmployee() { UserName = "TEST_USER" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Employee.Branch == "-6666");
            Assert.IsTrue(response.Employee.Department == DEPARTMENT);
            Assert.IsTrue(response.Employee.Manager == MANAGER);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.ReadEmployee(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ReadBranchView_AllScenario_ReturnsReadBranchViewResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.ReadBranchView(It.IsAny<string>()))
                .Returns(new List<BranchView>() { new BranchView() { Branch = "0666", UpdatedDate = "12/01/00", Active = true, UpdatedBy = "TEST_USER", EditMoney = Boolean.TrueString } });

            //Act
            var response = UserSecurityService.Get(new GetBranchView() { UserName = "TEST_USER" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.ViewableBranches.Any());
            Assert.IsTrue(response.ViewableBranches.First().Branch == "0666");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.ReadBranchView(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ResponseTest()
        {
            UserSecurity.API.ServiceModel.GetCompareScreenSecurityResponse resp = new ServiceModel.GetCompareScreenSecurityResponse();
            resp.ResponseStatus = new ServiceStack.ResponseStatus();
            resp.ResponseStatus.ErrorCode = "500";

            // assert
            // Assert.AreEqual(500, resp.ResponseStatus.ErrorCode);
            //  Assert.AreEqual("An error occurred whilst processing your request.", response.StatusDescription);
        }

        [TestMethod]
        public void CompareUserSecurity_AllScenario_ReturnsCompareUserSecurityResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CompareUserSecurity(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(new List<CompareSecurity>() { new CompareSecurity() { AccessLevel = "20", Default = 15, Id = 9999, Name = "SCREEN_NAME", SecurityLevels = new List<SecurityLevel>(), AccessLevel2 = "30",
                   SecurityLevels2 = new List<SecurityLevel>() } });

            //Act
            var response = UserSecurityService.Get(new GetCompareScreenSecurity() { UserNameA = "TEST_USERA", UserNameB = "Test_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.ComparedSecurities.Any());
            Assert.IsTrue(response.ComparedSecurities.First().AccessLevel == "20");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CompareUserSecurity(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void PostCloneScreenSecurity_AllScenario_ReturnsCloneScreenSecurityResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneUserScreenRights(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostCloneScreenSecurity() { UserNameA = "TEST_USERA", UserNameB = "Test_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneUserScreenRights(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void PostCloneBranch_AllScenario_ReturnsCloneBranchResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneBranch(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostCloneBranch() { UserNameA = "TEST_USERA", UserNameB = "TEST_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneBranch(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void PostGrantAllUserCerts_AllScenario_ReturnsGrantAllSecurityResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.GrantAllUserCerts(It.IsAny<string>()))
               .Returns(new List<UserCertification>() { new UserCertification() { EmpTrainingID = 1, EmpCode = "TEST", CourseID = 1, DatePassed = DateTime.Now, ExpirationDate = DateTime.Now, EnteredDate = DateTime.Now, EnteredBy = "Security" } });

            //Act
            var response = UserSecurityService.Post(new PostGrantAllUserCerts() { UserName = "TEST_USER" });

            //Assert
            Assert.IsNotNull(response);
            //  Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.GrantAllUserCerts(It.IsAny<string>()));
        }

        [TestMethod]
        public void PostCloneaAll_AllScenario_ReturnsCloneaAllResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneAll(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostCloneAll() { UserNameA = "TEST_USERA", UserNameB = "TEST_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneAll(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void PostCloneViewableBranches_AllScenario_ReturnsCloneViewableBranchesResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneViewableBranches(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostCloneViewableBranches() { UserNameA = "TEST_USERA", UserNameB = "TEST_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneViewableBranches(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void PostCloneTraining_AllScenario_ReturnsCloneTrainingResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneTraining(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostCloneTraining() { UserNameA = "TEST_USERA", UserNameB = "Test_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneTraining(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void PostMultipleCloneScreenSecurity_AllScenario_ReturnsMultipleCloneScreenSecurityResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneMultipleUserScreenRights(It.IsAny<string>(), It.IsAny<string>()))
               .Returns("Success");

            //Act
            var response = UserSecurityService.Post(new PostMultipleUserCloneScreenSecurity() { UserNameA = "TEST_USERA", UserNameB = "Test_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Message == "Success");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneMultipleUserScreenRights(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void PostMultipleCloneBranch_AllScenario_ReturnsMultipleCloneBranchResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneMultipleBranch(It.IsAny<string>(), It.IsAny<string>()))
               .Returns("Success");

            //Act
            var response = UserSecurityService.Post(new PostMultipleUserCloneBranch() { UserNameA = "TEST_USERA", UserNameB = "TEST_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Message == "Success");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneMultipleBranch(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void PostMultipleCloneAll_AllScenario_ReturnsCloneaMultipleAllResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneMultipleAll(It.IsAny<string>(), It.IsAny<string>()))
               .Returns("Success");

            //Act
            var response = UserSecurityService.Post(new PostMultipleUserCloneAll() { UserNameA = "TEST_USERA", UserNameB = "TEST_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Message == "Success");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneMultipleAll(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void PostMultipleCloneViewableBranches_AllScenario_ReturnsMultipleCloneViewableBranchesResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneMultipleViewableBranches(It.IsAny<string>(), It.IsAny<string>()))
               .Returns("Success");

            //Act
            var response = UserSecurityService.Post(new PostMultipleUserCloneViewableBranches() { UserNameA = "TEST_USERA", UserNameB = "TEST_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Message == "Success");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneMultipleViewableBranches(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void PostMultipleCloneTraining_AllScenario_ReturnsMultipleCloneTrainingResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.CloneMultipleTraining(It.IsAny<string>(), It.IsAny<string>()))
               .Returns("Success");

            //Act
            var response = UserSecurityService.Post(new PostMultipleUserCloneTraining() { UserNameA = "TEST_USERA", UserNameB = "Test_USERB" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Message == "Success");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.CloneMultipleTraining(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void PostUpdateScreenSecurity_AllScenario_ReturnsUpdateScreenSecurityResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.UpdateScreenSecurity(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
               .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostUpdateScreenSecurity() { UserName = "TEST_USER", ScreenID = -100, AccessLevel = -1 });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.UpdateScreenSecurity(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
        }



        [TestMethod]
        public void PostMoveBranch_AllScenario_ReturnsMoveBranchResponse()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.MoveBranch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostMoveBranch() { UserName = "TEST_USERA", Branch = "-6666", BranchManager = "TEST_MANAGER", Department = "TEST_DEPARTMENT" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.MoveBranch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void PostUpdateViewableBranches_AllScenario_ReturnsUpdateViewableBranches()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.UpdateViewableBranches(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
               .Returns("FailedBranches");

            //Act
            var response = UserSecurityService.Post(new PostUpdateViewableBranches() { UserName = "TEST_USERA", Branches = "1234", EditMoney = true });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Message == "FailedBranches");
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.UpdateViewableBranches(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
        }

        [TestMethod]
        public void PostDeleteViewableBranches_ReturnsDeletedBranches()
        {
            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.DeleteViewableBranches(It.IsAny<string>()))
               .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostDeleteViewableBranches() { UserName = "TEST_USERA" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.DeleteViewableBranches(It.IsAny<string>()));
        }

        [TestMethod]
        public void PostUpdateViewableBranch_AllScenario_ReturnsUpdateViewableBranchResponse()
        {
            var host = new BasicAppHost
            {
                TestMode = true,
                ConfigureContainer = container =>
                {
                    container.Register<IAuthSession>(c => new AuthUserSession
                    {
                        UserName = "Mocked",
                    });
                },

            }.Init();

            //Arrange
            MockUserSecurtiyManager.Setup(manager => manager.UpdateViewableBranch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<bool>()))
       .Returns(1);

            //Act
            var response = UserSecurityService.Post(new PostUpdateViewableBranch() { UserName = "TEST_USER", Active = true, BranchCode = "-6666", EditMoney = true, UpdatedBy = "TEST_USER" });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.RecordsUpdated == 1);
            Assert.IsNull(response.ResponseStatus);
            MockUserSecurtiyManager.Verify(manager => manager.UpdateViewableBranch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<bool>()));
            host.Dispose();
        }
        #endregion
    }
}
