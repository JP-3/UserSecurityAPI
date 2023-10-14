using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSecurity.API.ServiceModel;
using UserSecurity.API.ServiceModel.Messages;

namespace UserSecurity.API.Tests.IntegrationTests
{

    [TestClass]
    public class UserSecurityServiceTests
    {
        #region Properties
        private IServiceClient UserSecurityServiceClient { get; set; }

        private string TokenInfo { get; set; }
        private string userA = "YAOMIK";
        private string userB = "BRAUSAR";

        #endregion Properties

        [TestInitialize]
        public void InitializeTestData()
        {
            UserSecurityServiceClient = new JsonServiceClient(ConfigurationManager.AppSettings["UserSecurityServiceClientEndPoint"]);
        }

        [TestMethod, TestCategory("AutomatedAcceptance")]

        public void Read_UpdateViewableBranch()
        {

            ////Delete all the users viewable branches
            PostDeleteViewableBranches deleteViewableBranches = new PostDeleteViewableBranches();
            deleteViewableBranches.UserName = userA;
            UserSecurityServiceClient.Post(deleteViewableBranches);

            PostUpdateViewableBranch post = new PostUpdateViewableBranch();
            post.Active = true;
            post.BranchCode = "7652";
            post.EditMoney = true;
            post.UpdatedBy = "Security";
            post.UserName = userA;
            UserSecurityServiceClient.Post(post);

            GetBranchViewResponse response = new GetBranchViewResponse();
            GetBranchView request = new GetBranchView();
            request.UserName = userA;
            response = UserSecurityServiceClient.Get(request);

            var branch = response.ViewableBranches[0];

            if (response.ViewableBranches.Count < 0)
            {
                 Assert.Fail("Get_UpdateViewableBranch failed with unexpected results");
            }

            if (branch.Branch.Trim() != "7652" || branch.Active != true || branch.EditMoney != "True" || branch.UpdatedBy != "" || branch.UpdatedDate != DateTime.Now.ToString("MM/dd/yyyy") || response.ResponseStatus != null)
            {
                Assert.Fail("Get_UpdateViewableBranch failed with unexpected results");
            }
        }

        [TestMethod, TestCategory("AutomatedAcceptance")]

        public void Read_UpdateViewableBranches()
        {

            //Delete all the users viewable branches
            PostDeleteViewableBranches deleteViewableBranches = new PostDeleteViewableBranches();
            deleteViewableBranches.UserName = userA;
            UserSecurityServiceClient.Post(deleteViewableBranches);

            PostUpdateViewableBranches post = new PostUpdateViewableBranches();
            post.Branches = "7652, 2143";
            post.EditMoney = true;
            post.UserName = userA;
            UserSecurityServiceClient.Post(post);

            GetBranchViewResponse response = new GetBranchViewResponse();
            GetBranchView request = new GetBranchView();
            request.UserName = userA;
            response = UserSecurityServiceClient.Get(request);

            var branch = response.ViewableBranches[0];

            if (branch.Branch.Trim() != "2265" || branch.Active != true || branch.EditMoney != "True" || branch.UpdatedBy != "Security" || branch.UpdatedDate != DateTime.Now.ToString("MM/dd/yyyy") || response.ResponseStatus != null)
            {
                Assert.Fail("Get_UpdateViewableBranch failed with unexpected results");
            }

            branch = response.ViewableBranches[1];
            if (branch.Branch.Trim() != "8788" || branch.Active != true || branch.EditMoney != "True" || branch.UpdatedBy != "Security" || branch.UpdatedDate != DateTime.Now.ToString("MM/dd/yyyy") || response.ResponseStatus != null)
            {
                Assert.Fail("Get_UpdateViewableBranch failed with unexpected results");
            }
        }

        [TestMethod, TestCategory("AutomatedAcceptance")]
        public void Read_UpdateScreenSecurity()
        {
            GetScreenSecurityResponse response = new GetScreenSecurityResponse();
            GetScreenSecurity request = new GetScreenSecurity();
            request.UserName = userA;

            UpdateDefaultScreenAccessLevel(userA, 0, 171);

            //update to a new state
            PostUpdateScreenSecurity post = new PostUpdateScreenSecurity();
            post.UserName = userA;
            post.AccessLevel = 100;
            post.ScreenID = 171;

            UserSecurityServiceClient.Post(post);
            response = UserSecurityServiceClient.Get(request);
            int index = response.ScreenSecurities.FindIndex(screen171 => screen171.Id == 171);
            var screen = response.ScreenSecurities[index];

            if (screen.Id != 171 || screen.AccessLevel != "100" || screen.Default != 100 || screen.Name != "Billing Statistics" || response.ResponseStatus != null)
            {
                Assert.Fail("GetCompareScreenSecurity_Success failed with unexpected results");
            }
        }

        private void UpdateDefaultScreenAccessLevel(string userName, int accessLevel, int screenID)
        {
            //set the value to a default level 
            PostUpdateScreenSecurity post = new PostUpdateScreenSecurity();
            post.UserName = userName;
            post.AccessLevel = accessLevel;
            post.ScreenID = screenID;
            UserSecurityServiceClient.Post(post);

            //make sure it's in the correct state
            GetScreenSecurityResponse response = new GetScreenSecurityResponse();
            GetScreenSecurity request = new GetScreenSecurity();
            request.UserName = userA;
            response = UserSecurityServiceClient.Get(request);

            // screen 171 is in all environments
            int index = response.ScreenSecurities.FindIndex(updatedScreen => updatedScreen.Id == screenID);
            var screen = response.ScreenSecurities[index];

            if (screen.Id != screenID || screen.AccessLevel != accessLevel.ToString() || response.ResponseStatus != null)
            {
                Assert.Fail("GetCompareScreenSecurity_Success failed with unexpected results");
            }
        }

        [TestMethod, TestCategory("AutomatedAcceptance")]
        public void Read_UpdateCompareSplitScreenSecurity()
        {
            //clone users so they are identical
            PostMultipleUserCloneScreenSecurity post = new PostMultipleUserCloneScreenSecurity();
            post.UserNameA = userA;
            post.UserNameB = userB;
            UserSecurityServiceClient.Post(post);

            //get back screens
            GetCompareSplitScreenSecurityResponse response = new GetCompareSplitScreenSecurityResponse();
            GetCompareSplitScreenSecurity request = new GetCompareSplitScreenSecurity();
            request.UserNameA = userA;
            request.UserNameB = userB;
            response = UserSecurityServiceClient.Get(request);

            //make sure the users are identical
            if (response.SplitCompareSecurity.SponsorScreenSecurity.Count != 0 || response.SplitCompareSecurity.SponsorScreenSecurity.Count != 0 || response.ResponseStatus != null)
            {
                Assert.Fail("GetCompareSplitScreenSecurity_Success failed with unexpected results");
            }
        }

        [TestMethod, TestCategory("AutomatedAcceptance")]
        public void Read_UpdateGetEmployeeBranch()
        {
            PostMoveBranch post = new PostMoveBranch();
            post.Branch = "0000";
            post.BranchManager = "MANAGER";
            post.Department = "DEPARTMENT";
            post.UserName = userA;
            UserSecurityServiceClient.Post(post);

            GetEmployeeResponse response = new GetEmployeeResponse();
            GetEmployee request = new GetEmployee();
            request.UserName = userA;
            response = UserSecurityServiceClient.Get(request);

            if (response.Employee.Branch.Trim() != "0000" || response.Employee.Manager != "MANAGER" || response.Employee.Department != "DEPARTMENT" || response.ResponseStatus != null)
            {
                Assert.Fail("GetCompareSplitScreenSecurity_Success failed with unexpected results");
            }

            post.Branch = "1234";
            post.BranchManager = "PROUJOH";
            post.Department = "Test";
            post.UserName = userA;
            UserSecurityServiceClient.Post(post);

            response = UserSecurityServiceClient.Get(request);

            if (response.Employee.Branch.Trim() != "7652" || response.Employee.Manager != "PROUJOH" || response.Employee.Department != "Test" || response.ResponseStatus != null)
            {
                Assert.Fail("GetCompareSplitScreenSecurity_Success failed with unexpected results");
            }
        }

        [TestCleanup]
        public void CleanupTestData()
        {
            //  _fixture = null;
            UserSecurityServiceClient.Dispose();
        }
    }
}
