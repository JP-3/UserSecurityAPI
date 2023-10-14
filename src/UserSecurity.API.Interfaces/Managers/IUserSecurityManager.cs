using System.Collections.Generic;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.Interfaces.Managers
{
    public interface IUserSecurityManager
    {
        List<ScreenSecurity> ReadUserSecurity(string userName);

        List<CompareSecurity> CompareUserSecurity(string userName, string userNameB);

        List<BranchView> ReadBranchView(string userName);

        SplitScreenSecurity SplitScreenSecurity(string userName);

        SplitCompareSecurity SplitCompareSecurity(string userNameA, string userNameB);

        EmployeeInfo ReadEmployee(string userName);

        int MoveBranch(string userName, string branch, string branchManager, string department);

        int CloneTraining(string userNameA, string userNameB);

        int CloneBranch(string userNameA, string userNameB);

        int CloneUserScreenRights(string userNameA, string userNameB);

        int CloneViewableBranches(string userNameA, string userNameB);

        int CloneAll(string userNameA, string userNameB);

        string CloneMultipleTraining(string userNameA, string userNameB);

        string CloneMultipleBranch(string userNameA, string userNameB);

        string CloneMultipleUserScreenRights(string userNameA, string userNameB);

        string CloneMultipleViewableBranches(string userNameA, string userNameB);

        string CloneMultipleAll(string userNameA, string userNameB);

        int UpdateViewableBranch(string userName, string branchCode,bool active, string updatedBy, bool editMoney );

        int UpdateScreenSecurity(string userName, int screenID, int accessLevel);

        string UpdateViewableBranches(string userName, string branchCodes, bool editMoney);

        string CrossEnvironmentClone(string userName,  List<ScreenSecurity> screenSecurities) ;

        int DeleteViewableBranches(string userName);
        List<UserCertification> GrantAllUserCerts(string userName);
    }
}
