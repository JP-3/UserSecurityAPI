using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserSecurity.API.Interfaces.Database;
using UserSecurity.API.Repositories.Database;
using UserSecurity.API.Repositories;
using UserSecurity.API.Interfaces.Repositories;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace UserSecurity.API.Tests.UnitTests.Repositories
{
    [TestClass]
    public class ExpressRepositoryShould
    {
        #region Fields

        private Mock<ISqlConnection> _mockSqlConnection;
        private Mock<ISqlCommand> _mockSqlCommand;
        private Mock<ISqlDataReader> _mockSqlDataReader;
        private IExpressRepository _iExpressRepository;

        #endregion


        [TestInitialize]
        public void TestInit()
        {
            _mockSqlCommand = new Mock<ISqlCommand>();
            _mockSqlConnection = new Mock<ISqlConnection>();
            _mockSqlDataReader = new Mock<ISqlDataReader>();

        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void BillToRepository_SanityCheck()
        {
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            Assert.IsNotNull(_iExpressRepository);
        }

        [TestMethod]
        public void ReadScreens_ShouldReturnScreenData()
        {
            int screenID = 123;
            string screenName = "Screen1";
            int accessLevelDefault = 1;
            string sponsor = "SomeSponsor";
            int sponsorApprovalLevel = 100;

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r["ScreenID"]).Returns(screenID);
            _mockSqlDataReader.Setup(r => r["ScreenName"]).Returns(screenName);
            _mockSqlDataReader.Setup(r => r["AccessLevelDefault"]).Returns(accessLevelDefault);
            _mockSqlDataReader.Setup(r => r["Sponsor"]).Returns(sponsor);
            _mockSqlDataReader.Setup(r => r["SponsorApprovalLevel"]).Returns(sponsorApprovalLevel);

            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(_mockSqlCommand.Object.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance("dbo.GetScreensWithSecurity", It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            var screen = _iExpressRepository.ReadScreens();

            Assert.AreEqual(screen[0].ScreenID, screenID);
            Assert.AreEqual(screen[0].ScreenName, screenName);
            Assert.AreEqual(screen[0].AccessLevelDefault, accessLevelDefault);
            Assert.AreEqual(screen[0].Sponsor, sponsor);
            Assert.AreEqual(screen[0].SponsorApprovalLevel, sponsorApprovalLevel);
            _mockSqlCommand.Verify(s => s.GetNewInstance("dbo.GetScreensWithSecurity", It.IsAny<ISqlConnection>()), Times.Once);
        }

        [TestMethod]
        public void ReadScreenAccessLevels_ShouldReturnRowUpdated()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            int screenID = -1;
            int accessLevel = -3;
            string levelDescription = "levelDescription";

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r["ScreenID"]).Returns(screenID);
            _mockSqlDataReader.Setup(r => r["AccessLevel"]).Returns(accessLevel);
            _mockSqlDataReader.Setup(r => r["LevelDescription"]).Returns(levelDescription);
            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            var levels = _iExpressRepository.ReadScreenAccessLevels();

            Assert.AreEqual(levels[0].ScreenID, screenID);
            Assert.AreEqual(levels[0].AccessLevel, accessLevel);
            Assert.AreEqual(levels[0].LevelDescription, levelDescription);
        }

        [TestMethod]
        public void ReadEmployee_ShouldReturnEmployee()
        {
            string manager = "SomeGuy";
            string department = "MyDepartment";
            string branchCode = "6666";
            string firstName = "Joe";
            string lastName = "Blow";

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r["Manager"]).Returns(manager);
            _mockSqlDataReader.Setup(r => r["Department"]).Returns(department);
            _mockSqlDataReader.Setup(r => r["BranchCode"]).Returns(branchCode);
            _mockSqlDataReader.Setup(r => r["FirstName"]).Returns(firstName);
            _mockSqlDataReader.Setup(r => r["LastName"]).Returns(lastName);

            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(_mockSqlCommand.Object.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            var employee = _iExpressRepository.ReadEmployee("Joe");

            Assert.AreEqual(employee.Manager, manager);
            Assert.AreEqual(employee.Department, department);
        }

        [TestMethod]
        public void ReadUserScreenSecurities_ShouldReturnListOfSecurity()
        {
            int screenID = 123;
            int accessLevel = -900;

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r["ScreenID"]).Returns(screenID);
            _mockSqlDataReader.Setup(r => r["AccessLevel"]).Returns(accessLevel);

            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(_mockSqlCommand.Object.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            var screenSecurity = _iExpressRepository.ReadUserScreenSecurities("Joe");

            Assert.AreEqual(screenSecurity[0].ScreenID, screenID);
            Assert.AreEqual(screenSecurity[0].AccessLevel, accessLevel);
        }

        [TestMethod]
        public void ReadScreenAccessLevels_ShouldReturnListOfAccessLevels()
        {
            int screenID = 123;
            int accessLevel = -900;

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r["ScreenID"]).Returns(screenID);
            _mockSqlDataReader.Setup(r => r["AccessLevel"]).Returns(accessLevel);

            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(_mockSqlCommand.Object.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            var screenSecurity = _iExpressRepository.ReadUserScreenSecurities("Joe");

            Assert.AreEqual(screenSecurity[0].ScreenID, screenID);
            Assert.AreEqual(screenSecurity[0].AccessLevel, accessLevel);       
        }

        [TestMethod]
        public void ReadUserViewableBranches_ShouldReturnListOfViewableBranches()
        {
            string branchCode = "123";
            bool active = true;
            string updatedBy = "SOMEDUDE";
            string updatedDate = DateTime.Now.ToString("MM/dd/yyyy");
            bool editMoney = true;

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r["BranchCode"]).Returns(branchCode);
            _mockSqlDataReader.Setup(r => r["Active"]).Returns(active);
            _mockSqlDataReader.Setup(r => r["UpdatedBy"]).Returns(updatedBy);
            _mockSqlDataReader.Setup(r => r["UpdatedDate"]).Returns(updatedDate);
            _mockSqlDataReader.Setup(r => r["EditMoney"]).Returns(editMoney);

            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(_mockSqlCommand.Object.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            var viewableBranches = _iExpressRepository.ReadUserViewableBranches("Joe");

            Assert.AreEqual(viewableBranches[0].Branch, branchCode);
            Assert.AreEqual(viewableBranches[0].Active, active);
            Assert.AreEqual(viewableBranches[0].UpdatedBy, updatedBy);
            Assert.AreEqual(viewableBranches[0].UpdatedDate.ToString("MM/dd/yyyy"), updatedDate);
            Assert.AreEqual(viewableBranches[0].EditMoney, editMoney);
        }

        [TestMethod]
        public void MoveBranch_ShouldReturnRowUpdated()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            string userName = "Joe";
            string branch = "6666";
            string manager = "BigJoe";
            string department = "Test";

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            int recordsUpdated = _iExpressRepository.MoveBranch(userName, branch, manager, department);
            //Assert.AreEqual(recordsUpdated, 1);
        }

        [TestMethod]
        public void GrantAllTraining_ShouldReturnListofTrainingClasses()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            int trainingID = -1;
            string empCode = "BIGJOE";
            int courseID = 1234;
            DateTime datePassed = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            DateTime enteredDate = DateTime.Now;
            string enteredBy = "TESTUSER";

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r["EmpTrainingID"]).Returns(trainingID);
            _mockSqlDataReader.Setup(r => r["EmpCode"]).Returns(empCode);
            _mockSqlDataReader.Setup(r => r["CourseID"]).Returns(courseID);
            _mockSqlDataReader.Setup(r => r["DatePassed"]).Returns(datePassed);
            _mockSqlDataReader.Setup(r => r["ExpirationDate"]).Returns(expirationDate);
            _mockSqlDataReader.Setup(r => r["EnteredDate"]).Returns(enteredDate);
            _mockSqlDataReader.Setup(r => r["EnteredBy"]).Returns(enteredBy);

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            var trainingClasses = _iExpressRepository.GrantAllTraining(empCode);
            Assert.AreEqual(trainingClasses[0].EmpTrainingID, trainingID);
            Assert.AreEqual(trainingClasses[0].EmpCode, empCode);
            Assert.AreEqual(trainingClasses[0].CourseID, courseID);
            Assert.AreEqual(trainingClasses[0].DatePassed.ToString("MM/dd/yyyy"), datePassed.ToString("MM/dd/yyyy"));
            Assert.AreEqual(trainingClasses[0].ExpirationDate.ToString("MM/dd/yyyy"), expirationDate.ToString("MM/dd/yyyy"));
            Assert.AreEqual(trainingClasses[0].EnteredDate.ToString("MM/dd/yyyy"), enteredDate.ToString("MM/dd/yyyy"));
            Assert.AreEqual(trainingClasses[0].EnteredBy, enteredBy);
        }

        [TestMethod]
        public void CloneTraining_ShouldReturnRowUpdated()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            int recordsUpdated = _iExpressRepository.CloneTraining("USERA", "USERB");
            //Assert.AreEqual(recordsUpdated, 1);
        }


        [TestMethod]
        public void PurgeEmployeeBranchMoves_ShouldReturnRowUpdated()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
             int recordsUpdated = _iExpressRepository.PurgeEmployeeBranchMoves("USERB");
            //Assert.AreEqual(recordsUpdated, 1);
        }

        [TestMethod]
        public void CloneUserScreenRights_ShouldReturnRowUpdated()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            int recordsUpdated = _iExpressRepository.CloneUserScreenRights("USERA", "USERB");
            //Assert.AreEqual(recordsUpdated, 1);
        }

        [TestMethod]
        public void UpdateViewableBranch_ShouldReturnRowUpdated()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            int recordsUpdated = _iExpressRepository.UpdateViewableBranch("USERA", "6666", true, "USERB", true);
            //Assert.AreEqual(recordsUpdated, 1);
        }

        [TestMethod]
        public void UpdateScreenSecurity_ShouldReturnRowUpdated()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            int recordsUpdated = _iExpressRepository.UpdateScreenSecurity("USERA", -1, -10);
            //Assert.AreEqual(recordsUpdated, 1);
        }

        [TestMethod]
        public void DeleteViewableBranches_ShouldReturnRowUpdated()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true);
            _mockSqlCommand.Setup(s => s.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlCommand.Setup(s => s.Parameters).Returns(command.Parameters);
            _mockSqlCommand.Setup(s => s.GetNewInstance(It.IsAny<string>(), It.IsAny<ISqlConnection>())).Returns(_mockSqlCommand.Object);
            _mockSqlConnection.Setup(c => c.GetNewInstance(It.IsAny<string>())).Returns(_mockSqlConnection.Object);
            _iExpressRepository = new ExpressRepository(_mockSqlConnection.Object, _mockSqlCommand.Object);
            int recordsUpdated = _iExpressRepository.DeleteViewableBranches("USERA");
            //Assert.AreEqual(recordsUpdated, 1);
        }
    }
}


