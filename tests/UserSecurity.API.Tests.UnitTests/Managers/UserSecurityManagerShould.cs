using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PartyV5.ServiceModel.Messages;
using ServiceStack;
using UserSecurity.API.DataModels;
using UserSecurity.API.Interfaces.Managers;
using UserSecurity.API.Interfaces.Repositories;
using UserSecurity.API.Managers;

namespace UserSecurity.API.Tests.UnitTests.Managers
{
    [TestClass]
    public class UserSecurityManagerShould
    {
        #region Properties

        private IUserSecurityManager UserSecurityManager { get; set; }

        private Mock<IExpressRepository> MockExpressRepository { get; set; }
        private Mock<IPartyManager> MockPartyManager { get; set; }

        #endregion

        #region Setup

        [TestInitialize]
        public void TestInitialize()
        {
            MockExpressRepository = new Mock<IExpressRepository>();
            MockPartyManager = new Mock<IPartyManager>();
            UserSecurityManager = new UserSecurityManager(MockExpressRepository.Object, MockPartyManager.Object);

        }

        #endregion

        #region Public Methods      

        [TestMethod]
        public void ReadBranchView_AllScenario_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";

            MockExpressRepository.Setup(repository => repository.ReadUserViewableBranches(USER_NAME)).Returns(new List<UserBranchView>());

            //Act
            var response = UserSecurityManager.ReadBranchView(USER_NAME);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.ReadUserViewableBranches(USER_NAME), Times.Once());
        }


        [TestMethod]
        public void ReadUserSecurity_AllScenario_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";

            MockExpressRepository.Setup(repository => repository.ReadScreens()).Returns(new List<Screen>());
            MockExpressRepository.Setup(repository => repository.ReadUserScreenSecurities(USER_NAME))
                .Returns(new List<UserScreenSecurity>());
            MockExpressRepository.Setup(repository => repository.ReadScreenAccessLevels())
                .Returns(new List<ScreenAccessLevel>());

            //Act
            var response = UserSecurityManager.ReadUserSecurity(USER_NAME);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.ReadScreens(), Times.Once);
            MockExpressRepository.Verify(repository => repository.ReadUserScreenSecurities(USER_NAME), Times.Once());
            MockExpressRepository.Verify(repository => repository.ReadScreenAccessLevels(), Times.Once());
        }

        [TestMethod]
        public void Post_UpdateScreenSecurity_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";
            const int SCREEN_ID = -001;
            const int ACCESS_LEVEL = -1;

            MockExpressRepository.Setup(repository => repository.UpdateScreenSecurity(USER_NAME, SCREEN_ID, ACCESS_LEVEL)).Returns(new int());

            //Act
            var response = UserSecurityManager.UpdateScreenSecurity(USER_NAME, SCREEN_ID, ACCESS_LEVEL);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.UpdateScreenSecurity(USER_NAME, SCREEN_ID, ACCESS_LEVEL), Times.Once());
        }

        [TestMethod]
        public void Post_UpdateTrainingCerts_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";

            MockExpressRepository.Setup(repository => repository.GrantAllTraining(USER_NAME)).Returns(new List<UserCerts>() { new UserCerts() { EmpTrainingID = 1, EmpCode = "TEST", CourseID = 1, DatePassed = DateTime.Now, ExpirationDate = DateTime.Now, EnteredDate = DateTime.Now, EnteredBy = "Security" } });

            //Act
            var response = UserSecurityManager.GrantAllUserCerts(USER_NAME);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.GrantAllTraining(USER_NAME), Times.Once());
        }

        [TestMethod]
        public void Post_CloneTraining_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAMEA = "TEST_USERA";
            const string USER_NAMEB = "TEST_USERB";

            MockExpressRepository.Setup(repository => repository.CloneTraining(USER_NAMEA, USER_NAMEB)).Returns(new int());

            //Act
            var response = UserSecurityManager.CloneTraining(USER_NAMEA, USER_NAMEB);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.CloneTraining(USER_NAMEA, USER_NAMEB), Times.Once());
        }

        [TestMethod]
        public void Post_CloneUserScreenRights_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAMEA = "TEST_USERA";
            const string USER_NAMEB = "TEST_USERB";

            MockExpressRepository.Setup(repository => repository.CloneUserScreenRights(USER_NAMEA, USER_NAMEB)).Returns(new int());

            //Act
            var response = UserSecurityManager.CloneUserScreenRights(USER_NAMEA, USER_NAMEB);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.CloneUserScreenRights(USER_NAMEA, USER_NAMEB), Times.Once());
        }

        [TestMethod]
        public void Post_CloneBranch_CallsExpressRepository()
        {
            const string USER_NAMEA = "TEST_USERA";
            const string USER_NAMEB = "TEST_USERB";

            var employee = new Employee() { Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            MockExpressRepository.Setup(repository => repository.ReadEmployee(USER_NAMEA)).Returns(employee);

            var response = UserSecurityManager.CloneBranch(USER_NAMEA, USER_NAMEB);
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.MoveBranch(USER_NAMEB, employee.Branch, employee.Manager, employee.Department), Times.Once());
        }

        [TestMethod]
        public void Post_CloneMultipleBranch_CallsExpressRepository()
        {
            const string USER_NAMEA = "TEST_USERA";
            const string USER_NAMES = "TEST_USERB, TEST_USERC, Test_USERD";

            var employee = new Employee() { FirstName = "Joe", LastName = "Blow", Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            var employee2 = new Employee() { FirstName = null, LastName = null, Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            MockExpressRepository.Setup(repository => repository.ReadEmployee(USER_NAMEA)).Returns(employee);
            MockExpressRepository.Setup(repository => repository.ReadEmployee("TEST_USERB")).Returns(employee2);
            MockExpressRepository.Setup(repository => repository.ReadEmployee("TEST_USERC")).Returns(employee);

            var response = UserSecurityManager.CloneMultipleBranch(USER_NAMEA, USER_NAMES);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void Post_CloneMultipleTraining_CallsExpressRepository()
        {
            const string USER_NAMEA = "TEST_USERA";
            const string USER_NAMES = "TEST_USERB, TEST_USERC";
            var employee = new Employee() { FirstName = "Joe", LastName = "Blow", Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            var employee2 = new Employee() { FirstName = null, LastName = null, Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            MockExpressRepository.Setup(repository => repository.ReadEmployee(USER_NAMEA)).Returns(employee);
            MockExpressRepository.Setup(repository => repository.ReadEmployee("TEST_USERB")).Returns(employee2);
            MockExpressRepository.Setup(repository => repository.ReadEmployee("TEST_USERC")).Returns(employee);

            var response = UserSecurityManager.CloneMultipleTraining(USER_NAMEA, USER_NAMES);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void Post_CloneMultipleUserScreenRights_CallsExpressRepository()
        {
            const string USER_NAMEA = "TEST_USERA";
            const string USER_NAMES = "TEST_USERB, TEST_USERC";

            var response = UserSecurityManager.CloneMultipleUserScreenRights(USER_NAMEA, USER_NAMES);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void Post_CloneAll_CallsExpressRepository()
        {
            const string USER_NAMEA = "TEST_USERA";
            const string USER_NAMEA_BRANCH = "99999";
            const string USER_NAMEA_DEPARTMENT = "TEST_DEPARTMENT";
            const string USER_NAMEA_MANAGER = "TEST_MANAGER";
            const string USER_NAMEB = "TEST_USERB";

            MockExpressRepository.Setup(repository => repository.ReadEmployee(USER_NAMEA)).Returns(new Employee { Branch = USER_NAMEA_BRANCH, Department = USER_NAMEA_DEPARTMENT, Manager = USER_NAMEA_MANAGER });

            List<UserBranchView> branchView = new List<UserBranchView>();
            UserBranchView userBranchView = new UserBranchView();
            userBranchView.Branch = USER_NAMEA_BRANCH;
            userBranchView.Active = true;
            userBranchView.EditMoney = true;
            userBranchView.UpdatedBy = "TestUser";
            userBranchView.UpdatedDate = DateTime.MinValue;
            branchView.Add(userBranchView);
            MockExpressRepository.Setup(repository => repository.ReadUserViewableBranches(USER_NAMEA)).Returns(branchView);

            var response = UserSecurityManager.CloneAll(USER_NAMEA, USER_NAMEB);
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response);
        }

        [TestMethod]
        public void Post_CloneViewableBranches_CallsExpressRepository()
        {
            string user = "USERA";
            string userB = "USERB";
            string branch = "66666";
            string branchB = "7652";

            List<UserBranchView> branchView = new List<UserBranchView>();
            UserBranchView userBranchView = new UserBranchView();
            userBranchView.Branch = branch;
            userBranchView.Active = true;
            userBranchView.EditMoney = true;
            userBranchView.UpdatedBy = "TestUser";
            userBranchView.UpdatedDate = DateTime.MinValue;
            branchView.Add(userBranchView);
            userBranchView = new UserBranchView();
            userBranchView.Branch = branchB;
            userBranchView.Active = true;
            userBranchView.EditMoney = true;
            userBranchView.UpdatedBy = "TestUser";
            userBranchView.UpdatedDate = DateTime.MinValue;
            branchView.Add(userBranchView);
            MockExpressRepository.Setup(repository => repository.ReadUserViewableBranches(user)).Returns(branchView);

            var response = UserSecurityManager.CloneViewableBranches(user, userB);

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response);
        }

        [TestMethod]
        public void Post_CloneMultipleAll_CallsExpressRepository()
        {
            const string USER_NAMEA = "TEST_USERA";
            const string USER_NAMEA_BRANCH = "99999";
            const string USER_NAMEA_DEPARTMENT = "TEST_DEPARTMENT";
            const string USER_NAMEA_MANAGER = "TEST_MANAGER";
            const string USER_NAMEB = "TEST_USERB, TEST_USERC";
            const string USER_NAMEB_BRANCH = "123456";

            MockExpressRepository.Setup(repository => repository.ReadEmployee(USER_NAMEA)).Returns(new Employee
            {
                Branch = USER_NAMEA_BRANCH,
                Department = USER_NAMEA_DEPARTMENT,
                Manager = USER_NAMEA_MANAGER
            });

            List<UserBranchView> branchView = new List<UserBranchView>();
            UserBranchView userBranchView = new UserBranchView();
            userBranchView.Branch = USER_NAMEA_BRANCH;
            userBranchView.Active = true;
            userBranchView.EditMoney = true;
            userBranchView.UpdatedBy = "TestUser";
            userBranchView.UpdatedDate = DateTime.MinValue;
            branchView.Add(userBranchView);
            MockExpressRepository.Setup(repository => repository.ReadUserViewableBranches(USER_NAMEA)).Returns(branchView);

            branchView = new List<UserBranchView>();
            userBranchView = new UserBranchView();
            userBranchView.Branch = USER_NAMEB_BRANCH;
            userBranchView.Active = true;
            userBranchView.EditMoney = true;
            userBranchView.UpdatedBy = "TestUser";
            userBranchView.UpdatedDate = DateTime.MinValue;
            branchView.Add(userBranchView);
            MockExpressRepository.Setup(repository => repository.ReadUserViewableBranches("TEST_USERB")).Returns(branchView);

            var employee = new Employee() { FirstName = "Joe", LastName = "Blow", Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            var employee2 = new Employee() { FirstName = null, LastName = null, Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            MockExpressRepository.Setup(repository => repository.ReadEmployee("TEST_USERB")).Returns(employee);
            MockExpressRepository.Setup(repository => repository.ReadEmployee("TEST_USERC")).Returns(employee2);


            var response = UserSecurityManager.CloneMultipleAll(USER_NAMEA, USER_NAMEB);

            Assert.IsNotNull(response);
            Assert.AreEqual("Users are invalid: TEST_USERC", response);
        }

        [TestMethod]
        public void Post_CloneMultipleViewableBranches_CallsExpressRepository()
        {
            string user = "USERA";
            string userB = "USERB, USERC";
            string branch = "66666";
            string branchB = "7652";

            List<UserBranchView> branchView = new List<UserBranchView>();
            UserBranchView userBranchView = new UserBranchView();
            userBranchView.Branch = branch;
            userBranchView.Active = true;
            userBranchView.EditMoney = true;
            userBranchView.UpdatedBy = "TestUser";
            userBranchView.UpdatedDate = DateTime.MinValue;
            branchView.Add(userBranchView);
            userBranchView = new UserBranchView();
            userBranchView.Branch = branchB;
            userBranchView.Active = true;
            userBranchView.EditMoney = true;
            userBranchView.UpdatedBy = "TestUser";
            userBranchView.UpdatedDate = DateTime.MinValue;
            branchView.Add(userBranchView);
            MockExpressRepository.Setup(repository => repository.ReadUserViewableBranches(user)).Returns(branchView);

            var employee = new Employee() { FirstName = "Joe", LastName = "Blow", Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            var employee2 = new Employee() { FirstName = null, LastName = null, Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            MockExpressRepository.Setup(repository => repository.ReadEmployee("USERB")).Returns(employee);
            MockExpressRepository.Setup(repository => repository.ReadEmployee("USERC")).Returns(employee2);

            var response = UserSecurityManager.CloneMultipleViewableBranches(user, userB);

            Assert.IsNotNull(response);
            Assert.AreEqual("Users are invalid: USERC", response);
        }

        [TestMethod]
        public void Post_MoveBranch_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";
            const string BRANCH = "-6666";
            const string BRANCH_MANAGER = "TEST_MANAGER";
            const string DEPARTMENT = "TEST_DEPARTMENT";

            MockExpressRepository.Setup(repository => repository.MoveBranch(USER_NAME, BRANCH, BRANCH_MANAGER, DEPARTMENT)).Returns(new int());

            //Act
            var response = UserSecurityManager.MoveBranch(USER_NAME, BRANCH, BRANCH_MANAGER, DEPARTMENT);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.MoveBranch(USER_NAME, BRANCH, BRANCH_MANAGER, DEPARTMENT), Times.Once());
        }

        [TestMethod]
        public void Post_DeleteViewableBranches_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";

            MockExpressRepository.Setup(repository => repository.DeleteViewableBranches(USER_NAME)).Returns(new int());

            //Act
            var response = UserSecurityManager.DeleteViewableBranches(USER_NAME);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.DeleteViewableBranches(USER_NAME), Times.Once());
        }

        [TestMethod]
        public void Post_UpdateViewableBranch_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";
            const string BRANCH = "-6666";
            const bool ACTIVE = true;
            const string UPDATED_BY = "TEST_DEPARTMENT";
            const bool EDIT_MONEY = true;

            MockExpressRepository.Setup(repository => repository.UpdateViewableBranch(USER_NAME, BRANCH, ACTIVE, UPDATED_BY, EDIT_MONEY)).Returns(new int());

            //Act
            var response = UserSecurityManager.UpdateViewableBranch(USER_NAME, BRANCH, ACTIVE, UPDATED_BY, EDIT_MONEY);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.UpdateViewableBranch(USER_NAME, BRANCH, ACTIVE, UPDATED_BY, EDIT_MONEY), Times.Once());
        }

        [TestMethod]
        public void Post_UpdateViewableBranches_CallsExpressRepository()
        {
            string user = "USERA";
            string branch = "2143";
            string branchB = "7652";
            string branchC = "66666";

            MockExpressRepository.Setup(repository => repository.ReadUserViewableBranches(user)).Returns(new List<UserBranchView> {
                new UserBranchView { Branch = branch, Active = true, EditMoney = true, UpdatedBy = "TestUser", UpdatedDate = DateTime.MinValue},
                new UserBranchView { Branch = branchB, Active = true, EditMoney = true, UpdatedBy = "TestUser", UpdatedDate = DateTime.MinValue}});

            //Force the fail in the catch block
            var response = UserSecurityManager.UpdateViewableBranches(user, branch + ", " + branchC, true);
            Assert.IsNotNull(response);
            Assert.AreEqual($"Invalid Branches {branchC}", response);

            MockPartyManager.Setup(repository => repository.GetBranch(branch)).Returns(new ReadBranchResponse() { ResponseStatus = null });
            MockPartyManager.Setup(repository => repository.GetBranch(branchC)).Returns(new ReadBranchResponse() { ResponseStatus = null });

            //Add the branches and go through the try block
            response = UserSecurityManager.UpdateViewableBranches(user, branch + ", " + branchC, true);
            Assert.IsNotNull(response);
            Assert.AreEqual($"Invalid Branches", response);
        }

        [TestMethod]
        public void CompareUserSecurity_AllScenario_CallsExpressRepository()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";
            const string USER_NAME2 = "TEST_USER2";

            MockExpressRepository.Setup(repository => repository.ReadScreens()).Returns(new List<Screen>());
            MockExpressRepository.Setup(repository => repository.ReadUserScreenSecurities(USER_NAME))
                .Returns(new List<UserScreenSecurity>());
            MockExpressRepository.Setup(repository => repository.ReadScreenAccessLevels())
                .Returns(new List<ScreenAccessLevel>());

            //Act
            var response = UserSecurityManager.CompareUserSecurity(USER_NAME, USER_NAME2);

            //Assert
            Assert.IsNotNull(response);
            MockExpressRepository.Verify(repository => repository.ReadScreens(), Times.Once);
            MockExpressRepository.Verify(repository => repository.ReadUserScreenSecurities(USER_NAME), Times.Once());
            MockExpressRepository.Verify(repository => repository.ReadScreenAccessLevels(), Times.Once());
        }

        [TestMethod]
        public void ReadSplitScreenSecurity_ScreensExist_ReturnsSplitSecurity()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";

            const int SCREEN_ID = -1;
            const string SCREEN_NAME = "TEST_SCREEN_NAME";
            const int ACCESS_LEVEL_DEFAULT = -9998;
            const int ACCESS_LEVEL = -1010;

            const int SCREEN_ID2 = -2;
            const string SCREEN_NAME2 = "TEST_SCREEN_NAME2";
            const int ACCESS_LEVEL_DEFAULT2 = -9999;
            const string SPONSOR = "TEST_SPONSOR";
            const int SPONSOR_LEVEL = 20;

            var screens = new List<Screen>()
            { new Screen() { ScreenID = SCREEN_ID, ScreenName = SCREEN_NAME, AccessLevelDefault = ACCESS_LEVEL_DEFAULT, Sponsor = SPONSOR, SponsorApprovalLevel = SPONSOR_LEVEL},
             { new Screen() { ScreenID = SCREEN_ID, ScreenName = SCREEN_NAME, AccessLevelDefault = ACCESS_LEVEL_DEFAULT, Sponsor = "" } },
            { new Screen() { ScreenID = SCREEN_ID, ScreenName = SCREEN_NAME2, AccessLevelDefault = ACCESS_LEVEL_DEFAULT2 } } };

            var userScreenSecurity = new List<UserScreenSecurity> { new UserScreenSecurity() { ScreenID = SCREEN_ID, AccessLevel = ACCESS_LEVEL } };
            var screenAccessLevel = new List<ScreenAccessLevel> { new ScreenAccessLevel() { ScreenID = SCREEN_ID2, AccessLevel = -8888, LevelDescription = "TEST_LEVEL_DESCRIPTION" } };

            MockExpressRepository.Setup(repository => repository.ReadScreens()).Returns(screens);
            MockExpressRepository.Setup(repository => repository.ReadUserScreenSecurities(USER_NAME)).Returns(userScreenSecurity);
            MockExpressRepository.Setup(repository => repository.ReadScreenAccessLevels()).Returns(screenAccessLevel);

            //Act
            var response = UserSecurityManager.SplitScreenSecurity(USER_NAME);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(screens.First().ScreenID, response.StandardScreenSecurity.First().Id);
            Assert.AreEqual(screens.First().ScreenID, response.SponsorScreenSecurity.First().Id);
            Assert.AreEqual(screens.Last().Sponsor, response.StandardScreenSecurity.Last().Sponsor);
            Assert.AreEqual(screens.First().Sponsor, response.SponsorScreenSecurity.First().Sponsor);
            Assert.AreEqual(screens.Last().SponsorApprovalLevel, response.StandardScreenSecurity.Last().SponsorScreenApproval);
            Assert.AreEqual(screens.First().SponsorApprovalLevel, response.SponsorScreenSecurity.First().SponsorScreenApproval);
        }


        [TestMethod]
        public void ReadUserSecurity_ScreensExist_ReturnsScreenSecurity()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";
            const int SCREEN_ID = -1;
            const string SCREEN_NAME = "TEST_SCREEN_NAME";
            const int ACCESS_LEVEL_DEFAULT = -9999;
            const int ACCESS_LEVEL = -1010;

            var screens = new List<Screen> { new Screen() { ScreenID = SCREEN_ID, ScreenName = SCREEN_NAME, AccessLevelDefault = ACCESS_LEVEL_DEFAULT } };
            var userScreenSecurity = new List<UserScreenSecurity> { new UserScreenSecurity() { ScreenID = SCREEN_ID, AccessLevel = ACCESS_LEVEL } };
            var screenAccessLevel = new List<ScreenAccessLevel> { new ScreenAccessLevel() { ScreenID = SCREEN_ID, AccessLevel = -8888, LevelDescription = "TEST_LEVEL_DESCRIPTION" } };

            MockExpressRepository.Setup(repository => repository.ReadScreens()).Returns(screens);
            MockExpressRepository.Setup(repository => repository.ReadUserScreenSecurities(USER_NAME)).Returns(userScreenSecurity);
            MockExpressRepository.Setup(repository => repository.ReadScreenAccessLevels()).Returns(screenAccessLevel);

            //Act
            var response = UserSecurityManager.ReadUserSecurity(USER_NAME);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(screens.First().ScreenID, response.First().Id);
            Assert.AreEqual(screens.First().ScreenName, response.First().Name);
            Assert.AreEqual(screens.First().AccessLevelDefault, response.First().Default);
            Assert.AreEqual(ACCESS_LEVEL, int.Parse(response.First().AccessLevel));
            Assert.AreEqual(screenAccessLevel.First().LevelDescription, response.First().SecurityLevels.First().LevelDescription);
            Assert.AreEqual(screenAccessLevel.First().AccessLevel, int.Parse(response.First().SecurityLevels.First().AccessLevel));
        }

        [TestMethod]
        public void ReadBranchView_ScreensExist_ReturnsBranchView()
        {
            const string USER_NAME = "TEST_USER";
            const string BRANCH = "0666";
            const bool ACTIVE = true;
            const bool EDIT_MONEY = true;

            var branches = new List<UserBranchView> { new UserBranchView() { Branch = BRANCH, Active = ACTIVE, EditMoney = EDIT_MONEY, UpdatedBy = USER_NAME, UpdatedDate = DateTime.MinValue } };
            MockExpressRepository.Setup(repository => repository.ReadUserViewableBranches(USER_NAME)).Returns(branches);

            var response = UserSecurityManager.ReadBranchView(USER_NAME);

            var v = DateTime.Parse(response.First().UpdatedDate);

            Assert.IsNotNull(response);
            Assert.AreEqual(branches.First().Branch, response.First().Branch);
            Assert.AreEqual(branches.First().Active, response.First().Active);
            Assert.AreEqual(branches.First().EditMoney, bool.Parse(response.First().EditMoney));
            Assert.AreEqual(branches.First().UpdatedBy, response.First().UpdatedBy);
            Assert.AreEqual(branches.First().UpdatedDate, DateTime.Parse(response.First().UpdatedDate)); //wasn't sure how to handle this     
        }

        [TestMethod]
        public void CompareUserSecurity_ScreensExist_ReturnsUserSecurity()
        {
            //Arrange
            const string USER_NAME = "TEST_USER";
            const string USER_NAME2 = "TEST_USER2";

            const int SCREEN_ID = -1;
            const string SCREEN_NAME = "TEST_SCREEN_NAME";
            const int ACCESS_LEVEL_DEFAULT = -9998;
            const int ACCESS_LEVEL = -1010;

            const int SCREEN_ID2 = -2;
            const string SCREEN_NAME2 = "TEST_SCREEN_NAME2";
            const int ACCESS_LEVEL_DEFAULT2 = -9999;
            const int ACCESS_LEVEL2 = -1011;


            var screens = new List<Screen>
            { new Screen() { ScreenID = SCREEN_ID, ScreenName = SCREEN_NAME, AccessLevelDefault = ACCESS_LEVEL_DEFAULT, Sponsor = "TEST_SPONSOR", SponsorApprovalLevel = 50 },
            { new Screen() { ScreenID = SCREEN_ID2, ScreenName = SCREEN_NAME2, AccessLevelDefault = ACCESS_LEVEL_DEFAULT2 , Sponsor = "TEST_SPONSOR", SponsorApprovalLevel = 20 } } };

            var userScreenSecurity = new List<UserScreenSecurity>()
            { new UserScreenSecurity() { ScreenID = SCREEN_ID, AccessLevel = ACCESS_LEVEL } ,
            { new UserScreenSecurity() { ScreenID = SCREEN_ID2, AccessLevel = ACCESS_LEVEL } } };

            var userScreenSecurity2 = new List<UserScreenSecurity>()
            { new UserScreenSecurity() { ScreenID = SCREEN_ID, AccessLevel = ACCESS_LEVEL },
            { new UserScreenSecurity() { ScreenID = SCREEN_ID2, AccessLevel = ACCESS_LEVEL2 } } };

            var screenAccessLevel = new List<ScreenAccessLevel> { new ScreenAccessLevel() { ScreenID = SCREEN_ID2, AccessLevel = -8888, LevelDescription = "TEST_LEVEL_DESCRIPTION" } };
            var screenAccessLevel2 = new List<ScreenAccessLevel> { new ScreenAccessLevel() { ScreenID = SCREEN_ID2, AccessLevel = -8888, LevelDescription = "TEST_LEVEL_DESCRIPTION" } };

            MockExpressRepository.Setup(repository => repository.ReadScreens()).Returns(screens);
            MockExpressRepository.Setup(repository => repository.ReadUserScreenSecurities(USER_NAME)).Returns(userScreenSecurity);
            MockExpressRepository.Setup(repository => repository.ReadUserScreenSecurities(USER_NAME2)).Returns(userScreenSecurity2);
            MockExpressRepository.Setup(repository => repository.ReadScreenAccessLevels()).Returns(screenAccessLevel);

            //Act
            var response = UserSecurityManager.CompareUserSecurity(USER_NAME, USER_NAME2);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(screens.Last().ScreenID, response.First().Id);
            Assert.AreEqual(screens.Last().ScreenName, response.First().Name);
            Assert.AreEqual(screens.Last().AccessLevelDefault, response.First().Default);
            Assert.AreEqual(ACCESS_LEVEL, int.Parse(response.Last().AccessLevel));
            Assert.AreEqual(ACCESS_LEVEL2, int.Parse(response.Last().AccessLevel2));
            Assert.AreEqual(screenAccessLevel.First().LevelDescription, response.First().SecurityLevels.First().LevelDescription);
            Assert.AreEqual(screenAccessLevel.First().AccessLevel, int.Parse(response.First().SecurityLevels.First().AccessLevel));
            Assert.AreEqual(screenAccessLevel2.First().LevelDescription, response.First().SecurityLevels2.First().LevelDescription);
            Assert.AreEqual(screenAccessLevel2.First().AccessLevel, int.Parse(response.First().SecurityLevels2.First().AccessLevel));
        }


        [TestMethod]
        public void ReadEmployee_ReturnsEmployee()
        {
            //Arrange
            var employee = new Employee() { Branch = "-6666", Department = "TEST_DEPARTMENT", Manager = "SOME_MANAGER" };
            MockExpressRepository.Setup(repository => repository.ReadEmployee("TEST_USER")).Returns(employee);

            //Act
            var response = UserSecurityManager.ReadEmployee("TEST_USER");

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(employee.Branch, response.Branch);
            Assert.AreEqual(employee.Department, response.Department);
            Assert.AreEqual(employee.Manager, response.Manager);
        }

        [TestMethod]
        public void ReadSplitCompareSecurity_ReturnsSplitCompareSecurity()
        {
            const string USER_NAME = "TEST_USER";
            const string USER_NAME2 = "TEST_USER2";

            const int SCREEN_ID = -1;
            const string SCREEN_NAME = "TEST_SCREEN_NAME";
            const int ACCESS_LEVEL_DEFAULT = -9998;
            const int ACCESS_LEVEL = -1010;

            const int SCREEN_ID2 = -2;
            const string SCREEN_NAME2 = "TEST_SCREEN_NAME2";
            const int ACCESS_LEVEL_DEFAULT2 = -9999;
            const int ACCESS_LEVEL2 = -1011;
            const string SPONSOR = "TEST_SPONSOR";
            const int SPONSOR_LEVEL = 20;


            var screens = new List<Screen>()
            { new Screen() { ScreenID = SCREEN_ID, ScreenName = SCREEN_NAME, AccessLevelDefault = ACCESS_LEVEL_DEFAULT, Sponsor = SPONSOR, SponsorApprovalLevel = SPONSOR_LEVEL},
             { new Screen() { ScreenID = SCREEN_ID, ScreenName = SCREEN_NAME, AccessLevelDefault = ACCESS_LEVEL_DEFAULT, Sponsor = "" } },
            { new Screen() { ScreenID = SCREEN_ID2, ScreenName = SCREEN_NAME2, AccessLevelDefault = ACCESS_LEVEL_DEFAULT2 } } };

            var userScreenSecurity = new List<UserScreenSecurity> { new UserScreenSecurity() { ScreenID = SCREEN_ID, AccessLevel = ACCESS_LEVEL } };
            var userScreenSecurityB = new List<UserScreenSecurity> { new UserScreenSecurity() { ScreenID = SCREEN_ID, AccessLevel = ACCESS_LEVEL2 } };
            var screenAccessLevel = new List<ScreenAccessLevel> { new ScreenAccessLevel() { ScreenID = SCREEN_ID2, AccessLevel = -8888, LevelDescription = "TEST_LEVEL_DESCRIPTION" } };

            MockExpressRepository.Setup(repository => repository.ReadScreens()).Returns(screens);
            MockExpressRepository.Setup(repository => repository.ReadUserScreenSecurities(USER_NAME)).Returns(userScreenSecurity);
            MockExpressRepository.Setup(repository => repository.ReadUserScreenSecurities(USER_NAME2)).Returns(userScreenSecurityB);
            MockExpressRepository.Setup(repository => repository.ReadScreenAccessLevels()).Returns(screenAccessLevel);

            //Act
            var response = UserSecurityManager.SplitCompareSecurity(USER_NAME, USER_NAME2);
            Assert.IsNotNull(response);

            Assert.AreEqual(ACCESS_LEVEL.ToString(), response.SponsorScreenSecurity.First().AccessLevel);
            Assert.AreEqual(ACCESS_LEVEL2.ToString(), response.SponsorScreenSecurity.First().AccessLevel2);
            Assert.AreEqual(ACCESS_LEVEL.ToString(), response.StandardScreenSecurity.First().AccessLevel);
            Assert.AreEqual(ACCESS_LEVEL2.ToString(), response.StandardScreenSecurity.First().AccessLevel2);

        }
        #endregion
    }
}
