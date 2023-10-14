using System.Collections.Generic;
using System.Data;
using UserSecurity.API.DataModels;

namespace UserSecurity.API.Interfaces.Repositories
{
    public interface IExpressRepository
    {
        List<Screen> ReadScreens();

        List<UserScreenSecurity> ReadUserScreenSecurities(string userName);

        List<ScreenAccessLevel> ReadScreenAccessLevels();

        List<UserBranchView> ReadUserViewableBranches(string userName);

        int MoveBranch(string userName, string branch, string branchManager, string department);

        int CloneTraining(string userNameA, string userNameB);

        int CloneUserScreenRights(string userNameA, string userNameB);

        int UpdateViewableBranch(string userName, string branchCode, bool active, string updatedBy, bool editMoney);

        int UpdateScreenSecurity(string userName, int screenID, int accessLevel);

        List<UserCerts> GrantAllTraining(string userName);

        Employee ReadEmployee(string userName);

        int DeleteViewableBranches(string userName);

        int PurgeEmployeeBranchMoves(string userName);

        int UpdateScreenRight(string empCode, string name, int accessLevel);

        int DeleteAllScreenRights(string empCode);
    }
}
