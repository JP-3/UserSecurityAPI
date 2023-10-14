using ServiceStack;
using ServiceStack.Auth;
using UserSecurity.API.ServiceModel.Messages;
using UserSecurity.API.Interfaces.Managers;
using UserSecurity.API.ServiceModel;

namespace UserSecurity.API.ServiceDefinition
{
    public class UserSecurityService : Service
    {
        private IUserSecurityManager _userSecurityManager;

        public UserSecurityService(IUserSecurityManager userSecurityManager)
        {
            _userSecurityManager = userSecurityManager;
        }

        public GetScreenSecurityResponse Get(GetScreenSecurity request) =>
            new GetScreenSecurityResponse { ScreenSecurities = _userSecurityManager.ReadUserSecurity(request.UserName) };

        public GetCompareScreenSecurityResponse Get(GetCompareScreenSecurity request) =>
            new GetCompareScreenSecurityResponse { ComparedSecurities = _userSecurityManager.CompareUserSecurity(request.UserNameA, request.UserNameB) };

        //public GetBranchViewResponse Get(GetBranchView request) =>
        //  new GetBranchViewResponse { ViewableBranches = _userSecurityManager.ReadBranchView(request.UserName) };
        public GetBranchViewResponse Get(GetBranchView request)
        {
            var response = new GetBranchViewResponse();
            response.ViewableBranches = _userSecurityManager.ReadBranchView((request.UserName));
            return response;
        }

        public GetSplitScreenSecurityResponse Get(GetSplitScreenSecurity request) =>
          new GetSplitScreenSecurityResponse { SplitScreenSecurity = _userSecurityManager.SplitScreenSecurity(request.UserName) };

        public GetCompareSplitScreenSecurityResponse Get(GetCompareSplitScreenSecurity request) =>
        new GetCompareSplitScreenSecurityResponse { SplitCompareSecurity = _userSecurityManager.SplitCompareSecurity(request.UserNameA, request.UserNameB) };

        public GetEmployeeResponse Get(GetEmployee request) =>
        new GetEmployeeResponse { Employee = _userSecurityManager.ReadEmployee(request.UserName) };

      
        public PostResponse Post(PostMoveBranch request) =>
        new PostResponse { RecordsUpdated = _userSecurityManager.MoveBranch(request.UserName, request.Branch, request.BranchManager, request.Department) };

      
        public PostResponse Post(PostCloneTraining request) =>
        new PostResponse { RecordsUpdated = _userSecurityManager.CloneTraining(request.UserNameA, request.UserNameB) };

      
        public PostResponse Post(PostCloneBranch request) =>
        new PostResponse { RecordsUpdated = _userSecurityManager.CloneBranch(request.UserNameA, request.UserNameB) };

      
        public PostUserCertUpdateResponse Post(PostGrantAllUserCerts request) =>
        new PostUserCertUpdateResponse { UserCertificationList = _userSecurityManager.GrantAllUserCerts(request.UserName)};

      
        public PostResponse Post(PostCloneScreenSecurity request) =>
        new PostResponse { RecordsUpdated = _userSecurityManager.CloneUserScreenRights(request.UserNameA, request.UserNameB) };
        
        public PostResponse Post(PostUpdateViewableBranch request)
        {
            var response = new PostResponse();
            response.RecordsUpdated = _userSecurityManager.UpdateViewableBranch(request.UserName, request.BranchCode,
                request.Active, GetSession().GetUserName(), request.EditMoney);

            return response;
        }

        //public PostResponse Post(PostUpdateViewableBranch request) =>
        //new PostResponse { RecordsUpdated = _userSecurityManager.UpdateViewableBranch(request.UserName, request.BranchCode, request.Active, GetSession().GetUserName(), request.EditMoney) };


        public PostResponse Post(PostUpdateScreenSecurity request) =>
        new PostResponse { RecordsUpdated = _userSecurityManager.UpdateScreenSecurity(request.UserName, request.ScreenID, request.AccessLevel) };

      
        public PostResponse Post(PostUpdateViewableBranches request) =>
        new PostResponse { Message = _userSecurityManager.UpdateViewableBranches(request.UserName, request.Branches, request.EditMoney) };

      
        public PostResponse Post(PostCloneViewableBranches request) =>
        new PostResponse { RecordsUpdated = _userSecurityManager.CloneViewableBranches(request.UserNameA, request.UserNameB) };

      
        public PostResponse Post(PostCloneAll request) =>
        new PostResponse { RecordsUpdated = _userSecurityManager.CloneAll(request.UserNameA, request.UserNameB) };

      
        public PostResponse Post(PostMultipleUserCloneAll request) =>
        new PostResponse { Message = _userSecurityManager.CloneMultipleAll(request.UserNameA, request.UserNameB) };

      
        public PostResponse Post(PostMultipleUserCloneBranch request) =>
        new PostResponse { Message = _userSecurityManager.CloneMultipleBranch(request.UserNameA, request.UserNameB) };


        //public PostResponse Post(PostMultipleUserCloneScreenSecurity request) =>
        //new PostResponse { Message = _userSecurityManager.CloneMultipleUserScreenRights(request.UserNameA, request.UserNameB) };

        public PostResponse Post(PostMultipleUserCloneScreenSecurity request)
        {
            var response = new PostResponse();

            response.Message = _userSecurityManager.CloneMultipleUserScreenRights(request.UserNameA, request.UserNameB);
            return response;
        }

        public PostResponse Post(PostMultipleUserCloneTraining request) =>
        new PostResponse { Message = _userSecurityManager.CloneMultipleTraining(request.UserNameA, request.UserNameB) };

      
        public PostResponse Post(PostMultipleUserCloneViewableBranches request) =>
        new PostResponse { Message = _userSecurityManager.CloneMultipleViewableBranches(request.UserNameA, request.UserNameB) };


        public PostResponse Post(PostDeleteViewableBranches request) =>
        new PostResponse { RecordsUpdated = _userSecurityManager.DeleteViewableBranches(request.UserName) };

        public PostResponse Post(PostCrossEnvironmentClone request) =>
        new PostResponse {Message = _userSecurityManager.CrossEnvironmentClone(request.CloneToUsername, request.ScreenSecurities) };
    }
}
