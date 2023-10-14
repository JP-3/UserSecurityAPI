using System;
using System.Collections.Generic;
using System.Linq;
using UserSecurity.API.DataModels;
using UserSecurity.API.Interfaces.Managers;
using UserSecurity.API.Interfaces.Repositories;
using UserSecurity.API.ServiceModel.Types;
using PartyV5.ServiceModel.Messages;
using ServiceStack;

namespace UserSecurity.API.Managers
{
    public class UserSecurityManager : IUserSecurityManager
    {
        private IExpressRepository _expressRepository;
        private IPartyManager _partyManager;

        public UserSecurityManager(IExpressRepository expressRepository, IPartyManager partyManager)
        {
            _expressRepository = expressRepository;
            _partyManager = partyManager;
        }

        SplitScreenSecurity IUserSecurityManager.SplitScreenSecurity(string userName)
        {
            SplitScreenSecurity response = new SplitScreenSecurity();
            List<ScreenSecurity> standardScreenSecurity = new List<ScreenSecurity>();
            List<ScreenSecurity> sponsorScreenSecurity = new List<ScreenSecurity>();
            ScreenSecurity screenSecurity;
            List<Screen> screens = _expressRepository.ReadScreens();
            List<ScreenAccessLevel> screenAccessLevel = _expressRepository.ReadScreenAccessLevels();
            List<UserScreenSecurity> userScreenSecurity = _expressRepository.ReadUserScreenSecurities(userName);

            foreach (Screen screen in screens)
            {
                screenSecurity = new ScreenSecurity();

                //If you don't have a sponsor then it's a standard security screen
                if ((screen.Sponsor == string.Empty && screen.SponsorApprovalLevel == 0) || screen.Sponsor == null)
                {
                    standardScreenSecurity.Add(AddScreen(screenSecurity, screen, screenAccessLevel));
                }
                else //it's a sponsor screen and has elevated rights to be updated
                {
                    sponsorScreenSecurity.Add(AddScreen(screenSecurity, screen, screenAccessLevel));
                }

                screenSecurity.AccessLevel = "0";
                if (userScreenSecurity.Exists(user => user.ScreenID == screen.ScreenID))
                {
                    screenSecurity.AccessLevel = userScreenSecurity.Find(user => user.ScreenID == screen.ScreenID).AccessLevel.ToString();
                }

                screenSecurity.SecurityLevels = AddSecurityLevelsToScreen(screen, screenAccessLevel);
            }

            response.StandardScreenSecurity = standardScreenSecurity;
            response.SponsorScreenSecurity = sponsorScreenSecurity;
            return response;
        }

        SplitCompareSecurity IUserSecurityManager.SplitCompareSecurity(string userNameA, string userNameB)
        {
            SplitCompareSecurity response = new SplitCompareSecurity();
            CompareSecurity compareSecurity = new CompareSecurity();
            List<CompareSecurity> standardScreenSecurity = new List<CompareSecurity>();
            List<CompareSecurity> sponsorScreenSecurity = new List<CompareSecurity>();
            List<Screen> screens = _expressRepository.ReadScreens();
            List<UserScreenSecurity> userScreenSecurityA = _expressRepository.ReadUserScreenSecurities(userNameA);
            List<UserScreenSecurity> userScreenSecurityB = _expressRepository.ReadUserScreenSecurities(userNameB);
            List<SecurityLevel> securityLevels;
            List<ScreenAccessLevel> screenAccessLevel = _expressRepository.ReadScreenAccessLevels();

            foreach (var screen in screens)
            {
                compareSecurity = new CompareSecurity();
                securityLevels = new List<SecurityLevel>();

                //Add the default screen properties
                compareSecurity.Id = screen.ScreenID;
                compareSecurity.Name = screen.ScreenName;
                compareSecurity.Default = screen.AccessLevelDefault;
                compareSecurity.Sponsor = screen.Sponsor;
                compareSecurity.SponsorScreenApproval = screen.SponsorApprovalLevel;

                //Add all the security levels available for the Screen
                compareSecurity.SecurityLevels = AddSecurityLevelsToScreen(screen, screenAccessLevel);

                //Add the users access level to the response
                compareSecurity.AccessLevel = "0";
                if (userScreenSecurityA.Exists(user => user.ScreenID == screen.ScreenID))
                {
                    compareSecurity.AccessLevel = userScreenSecurityA.Find(user => user.ScreenID == screen.ScreenID).AccessLevel.ToString();
                }

                //Add all the security levels available to for the Screen    
                compareSecurity.SecurityLevels = AddSecurityLevelsToScreen(screen, screenAccessLevel);

                //Add the second users access level to the response
                compareSecurity.AccessLevel2 = "0";
                if (userScreenSecurityB.Exists(user => user.ScreenID == screen.ScreenID))
                {
                    compareSecurity.AccessLevel2 = userScreenSecurityB.Find(user => user.ScreenID == screen.ScreenID).AccessLevel.ToString();
                }

                //Add all the security levels available to for the Screen of the second user
                compareSecurity.SecurityLevels2 = AddSecurityLevelsToScreen(screen, screenAccessLevel);

                if ((screen.Sponsor == string.Empty && screen.SponsorApprovalLevel == 0) || screen.Sponsor == null)
                {
                    standardScreenSecurity.Add(compareSecurity);
                }
                else //it's a sponsor screen and has elevated rights to be updated
                {
                    sponsorScreenSecurity.Add(compareSecurity);
                }
            }

            RemoveDuplicateMatches(standardScreenSecurity);
            RemoveDuplicateMatches(sponsorScreenSecurity);
            response.StandardScreenSecurity = standardScreenSecurity;
            response.SponsorScreenSecurity = sponsorScreenSecurity;
            return response;
        }

        List<CompareSecurity> IUserSecurityManager.CompareUserSecurity(string userName, string userName2)
        {
            CompareSecurity compareSecurity;
            List<CompareSecurity> response = new List<CompareSecurity>();
            List<Screen> screens = _expressRepository.ReadScreens();
            List<UserScreenSecurity> userScreenSecurity = _expressRepository.ReadUserScreenSecurities(userName);
            List<UserScreenSecurity> userScreenSecurity2 = _expressRepository.ReadUserScreenSecurities(userName2);
            List<SecurityLevel> securityLevels;
            List<ScreenAccessLevel> screenAccessLevel = _expressRepository.ReadScreenAccessLevels();

            foreach (var screen in screens)
            {
                compareSecurity = new CompareSecurity();
                securityLevels = new List<SecurityLevel>();

                //Add the default screen properties
                compareSecurity.Id = screen.ScreenID;
                compareSecurity.Name = screen.ScreenName;
                compareSecurity.Default = screen.AccessLevelDefault;

                //Add the users access level to the response
                compareSecurity.AccessLevel = "0";
                if (userScreenSecurity.Exists(user => user.ScreenID == screen.ScreenID))
                {
                    compareSecurity.AccessLevel = userScreenSecurity.Find(user => user.ScreenID == screen.ScreenID).AccessLevel.ToString();
                }

                //Add all the security levels available for the Screen    
                compareSecurity.SecurityLevels = AddSecurityLevelsToScreen(screen, screenAccessLevel);

                //Add the second users access level to the response
                compareSecurity.AccessLevel2 = "0";
                if (userScreenSecurity.Exists(user => user.ScreenID == screen.ScreenID))
                {
                    compareSecurity.AccessLevel2 = userScreenSecurity2.Find(user => user.ScreenID == screen.ScreenID).AccessLevel.ToString();
                }

                //Add all the security levels available for the Screen of the second user
                compareSecurity.SecurityLevels2 = AddSecurityLevelsToScreen(screen, screenAccessLevel);
                response.Add(compareSecurity);
            }

            RemoveDuplicateMatches(response);
            return response;
        }

        List<ScreenSecurity> IUserSecurityManager.ReadUserSecurity(string userName)
        {
            ScreenSecurity screenSecurity;
            List<ScreenSecurity> response = new List<ScreenSecurity>();
            List<Screen> screens = _expressRepository.ReadScreens();
            List<UserScreenSecurity> userScreenSecurity = _expressRepository.ReadUserScreenSecurities(userName);

            var screenAccessLevel = _expressRepository.ReadScreenAccessLevels();

            foreach (var screen in screens)
            {
                screenSecurity = new ScreenSecurity();
                //Add the default screen properties
                screenSecurity.Id = screen.ScreenID;
                screenSecurity.Name = screen.ScreenName;
                screenSecurity.Default = screen.AccessLevelDefault;
                screenSecurity.Sponsor = screen.Sponsor;
                screenSecurity.SponsorScreenApproval = screen.SponsorApprovalLevel;

                //Add the users access level to the response   
                if (userScreenSecurity.Exists(user => user.ScreenID == screen.ScreenID))
                {
                    screenSecurity.AccessLevel = userScreenSecurity.Find(user => user.ScreenID == screen.ScreenID).AccessLevel.ToString();
                }

                //Add all the security levels available for the Screen
                screenSecurity.SecurityLevels = AddSecurityLevelsToScreen(screen, screenAccessLevel);
                response.Add(screenSecurity);
            }
            return response;
        }

        List<BranchView> IUserSecurityManager.ReadBranchView(string userName)
        {
            List<UserBranchView> userBranches = _expressRepository.ReadUserViewableBranches(userName);
            List<BranchView> branches = new List<BranchView>();
            BranchView branch;
            foreach (UserBranchView userBranch in userBranches)
            {
                branch = new BranchView();
                branch.Branch = userBranch.Branch;
                branch.Active = userBranch.Active;
                branch.UpdatedBy = userBranch.UpdatedBy;
                branch.UpdatedDate = userBranch.UpdatedDate.ToString("MM/dd/yyyy");
                branch.EditMoney = userBranch.EditMoney.ToString();
                branches.Add(branch);
            }
            return branches;
        }

        EmployeeInfo IUserSecurityManager.ReadEmployee(string userName)
        {
            EmployeeInfo employeeInfo = new EmployeeInfo();
            Employee employee = _expressRepository.ReadEmployee(userName);
            employeeInfo.Branch = employee.Branch;
            employeeInfo.Department = employee.Department;
            employeeInfo.Manager = employee.Manager;
            return employeeInfo;
        }

        int IUserSecurityManager.MoveBranch(string userName, string branch, string branchManager, string department)
        {
            _expressRepository.PurgeEmployeeBranchMoves(userName);
          return  _expressRepository.MoveBranch(userName, branch, branchManager, department);
        }

        private ScreenSecurity AddScreen(ScreenSecurity screenSecurity, Screen screen, List<ScreenAccessLevel> screenAccessLevel)
        {
            //Add the default screen properties
            screenSecurity.Id = screen.ScreenID;
            screenSecurity.Name = screen.ScreenName;
            screenSecurity.Default = screen.AccessLevelDefault;
            screenSecurity.Sponsor = screen.Sponsor;
            screenSecurity.SponsorScreenApproval = screen.SponsorApprovalLevel;

            //Add all the security levels available for the Screen
            screenSecurity.SecurityLevels = AddSecurityLevelsToScreen(screen, screenAccessLevel);
            return screenSecurity;
        }

        /// <summary>
        /// Add all the security levels available to for the Screen
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        private List<SecurityLevel> AddSecurityLevelsToScreen(Screen screen, List<ScreenAccessLevel> screenAccessLevel)
        {
            List<SecurityLevel> securityLevels = new List<SecurityLevel>();
            SecurityLevel securityLevel;

            foreach (ScreenAccessLevel level in screenAccessLevel)
            {
                securityLevel = new SecurityLevel();
                if (screen.ScreenID == level.ScreenID)
                {
                    securityLevel.LevelDescription = level.LevelDescription;
                    securityLevel.AccessLevel = level.AccessLevel.ToString();
                    securityLevels.Add(securityLevel);
                }
            }
            return securityLevels;
        }

        /// <summary>
        /// Remove any duplicate matches between userA and userB, leaving only differences
        /// </summary>
        /// <param name="compareSecurities"></param>
        /// <returns></returns>
        private List<CompareSecurity> RemoveDuplicateMatches(List<CompareSecurity> compareSecurities)
        {
            for (int i = compareSecurities.Count - 1; i >= 0; i--)
            {
                if (compareSecurities[i].AccessLevel == compareSecurities[i].AccessLevel2)
                {
                    compareSecurities.RemoveAt(i);
                }
            }
            return compareSecurities;
        }

        int IUserSecurityManager.UpdateViewableBranch(string userName, string branchCode, bool active, string updatedBy, bool editMoney)
        => _expressRepository.UpdateViewableBranch(userName, branchCode, active, updatedBy, editMoney);

        int IUserSecurityManager.UpdateScreenSecurity(string userName, int screenID, int accessLevel)
        => _expressRepository.UpdateScreenSecurity(userName, screenID, accessLevel);

        string IUserSecurityManager.UpdateViewableBranches(string userName, string branchCodes, bool editMoney)
        {
            string invalidBranches = "Invalid Branches";
            List<UserBranchView> userBranches = _expressRepository.ReadUserViewableBranches(userName);

            foreach (UserBranchView branch in userBranches)
            {
                branch.Branch = branch.Branch.Trim();
            }

            List<string> branches = branchCodes.Split(',').ToList();
            for (int i = 0; i < branches.Count; i++)
            {
                branches[i] = branches[i].Trim();

                if (!userBranches.Exists(branch => branch.Branch == branches[i]))
                {
                    try
                    {
                        if (_partyManager.GetBranch(branches[i]).ResponseStatus == null)
                        {
                            _expressRepository.UpdateViewableBranch(userName, branches[i], true, "Security", true);
                        }
                    }
                    catch
                    {
                        invalidBranches += $" {branches[i]}";
                    }
                }
            }
            return invalidBranches;
        }

        int IUserSecurityManager.CloneBranch(string userNameA, string userNameB)
        {
            Employee userA = _expressRepository.ReadEmployee(userNameA);
            _expressRepository.PurgeEmployeeBranchMoves(userNameB);
            return _expressRepository.MoveBranch(userNameB, userA.Branch, userA.Manager, userA.Department);
        }

        int IUserSecurityManager.CloneAll(string userNameA, string userNameB) => CloneAll(userNameA, userNameB);

        private int CloneAll(string userNameA, string userNameB)
        {
            _expressRepository.PurgeEmployeeBranchMoves(userNameB);
            Employee userA = _expressRepository.ReadEmployee(userNameA);
            int i = 0;
            i += _expressRepository.CloneTraining(userNameA, userNameB);
            i += _expressRepository.CloneUserScreenRights(userNameA, userNameB);
            i += _expressRepository.MoveBranch(userNameB, userA.Branch, userA.Manager, userA.Department);
            i += _expressRepository.CloneTraining(userNameA, userNameB);

            List<UserBranchView> branches = _expressRepository.ReadUserViewableBranches(userNameA);
            foreach (var branch in branches)
            {
                i += _expressRepository.UpdateViewableBranch(userNameB, branch.Branch, true, Environment.UserName.ToUpper(), branch.EditMoney);
            }

            return i;
        }

        int IUserSecurityManager.CloneTraining(string userNameA, string userNameB) => _expressRepository.CloneTraining(userNameA, userNameB);

        int IUserSecurityManager.CloneUserScreenRights(string userNameA, string userNameB) => _expressRepository.CloneUserScreenRights(userNameA, userNameB);

        int IUserSecurityManager.CloneViewableBranches(string userNameA, string userNameB)
        {
            List<UserBranchView> branches = _expressRepository.ReadUserViewableBranches(userNameA);
            int i = 0;
            foreach (var branch in branches)
            {
                i += _expressRepository.UpdateViewableBranch(userNameB, branch.Branch, true, Environment.UserName.ToUpper(), branch.EditMoney);
            }
            return i;
        }

        /// <summary>
        /// Takes in user to clone from and multiple users to clone to passing back a list of users that were invalid and updating any valid users
        /// </summary>
        /// <param name="userNameA"></param>
        /// <param name="userNamesB"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private string CloneMultipleUsers(string userNameA, string userNamesB, Func<string, string, int> method)
        {
            string invalidUsers = "Users are invalid: ";
            List<string> users = userNamesB.Split(',').ToList();
            for (int i = 0; i < users.Count; i++)
            {
                users[i] = users[i].Trim();
                if (users[i] != string.Empty)
                {
                    try
                    {
                        var employee = _expressRepository.ReadEmployee(users[i]);
                        if (employee.FirstName != null && employee.LastName !=null)
                        {
                            method(userNameA, users[i]);
                        }
                        else
                        {
                            invalidUsers += $"{users[i]}, ";
                        }
                    }
                    catch
                    {
                        invalidUsers += $"{users[i]}, ";
                    }
                }
            }
         return invalidUsers.TrimEnd(',', ' ');
        }

        private Employees GetValidUsers(string users)
        {
            Employees employees = new Employees();
            employees.InvalidEmployees = "Users are invalid: ";
            employees.ValidEmployees = users.Split(',').ToList();

            for (int i = employees.ValidEmployees.Count-1; i >= 0; i--)
            {
                employees.ValidEmployees[i] = employees.ValidEmployees[i].Trim();
                if (employees.ValidEmployees[i] != string.Empty)
                {
                    try
                    {
                        var employee = _expressRepository.ReadEmployee(employees.ValidEmployees[i]);

                        if (employee.FirstName == null && employee.LastName == null)
                        {
                            employees.InvalidEmployees += employees.ValidEmployees[i] + ", ";
                            employees.ValidEmployees.RemoveAt(i);
                        }
                    }
                    catch
                    {
                        employees.InvalidEmployees += employees.ValidEmployees[i] + ", ";
                        employees.ValidEmployees.RemoveAt(i);
                    }
                }
            }
            employees.InvalidEmployees = employees.InvalidEmployees.TrimEnd(',', ' ');
            return employees;
        }

        string IUserSecurityManager.CloneMultipleTraining(string userNameA, string userNameB) => CloneMultipleUsers(userNameA, userNameB, _expressRepository.CloneTraining);

        string IUserSecurityManager.CloneMultipleUserScreenRights(string userNameA, string userNameB) => CloneMultipleUsers(userNameA, userNameB, _expressRepository.CloneUserScreenRights);

        string IUserSecurityManager.CloneMultipleBranch(string userNameA, string userNameB)
        {
            Employee userA = _expressRepository.ReadEmployee(userNameA);
            Employees employees = GetValidUsers(userNameB);

            foreach (string employee in employees.ValidEmployees)
            {
                _expressRepository.PurgeEmployeeBranchMoves(employee);
                _expressRepository.MoveBranch(employee, userA.Branch, userA.Manager, userA.Department);
            }
            return employees.InvalidEmployees;
        }

        string IUserSecurityManager.CloneMultipleViewableBranches(string userNameA, string userNameB)
        {
            List<UserBranchView> branches = _expressRepository.ReadUserViewableBranches(userNameA);
            Employees employees = GetValidUsers(userNameB);

            foreach (string employee in employees.ValidEmployees)
            {
                foreach (var branch in branches)
                {
                    _expressRepository.UpdateViewableBranch(employee, branch.Branch, true, Environment.UserName.ToUpper(), branch.EditMoney);
                }
            }
            return employees.InvalidEmployees;
        }

        string IUserSecurityManager.CloneMultipleAll(string userNameA, string userNameB)
        {
            List<UserBranchView> branchesA = _expressRepository.ReadUserViewableBranches(userNameA);
            Employees employees = GetValidUsers(userNameB);
            Employee userA = _expressRepository.ReadEmployee(userNameA);

            foreach (string employee in employees.ValidEmployees)
            {
                _expressRepository.PurgeEmployeeBranchMoves(employee);
                _expressRepository.CloneTraining(userNameA, employee);
                _expressRepository.CloneUserScreenRights(userNameA, employee);
                _expressRepository.MoveBranch(employee, userA.Branch, userA.Manager, userA.Department);
                _expressRepository.CloneTraining(userNameA, employee);
         
                List<UserBranchView> branchesB = _expressRepository.ReadUserViewableBranches(employee);
                foreach (var branchA in branchesA)
                {
                    if (!branchesB.Exists(branch => branch.Branch == branchA.Branch))
                    {
                        _expressRepository.UpdateViewableBranch(employee, branchA.Branch, true, Environment.UserName.ToUpper(), branchA.EditMoney);
                    }
                }
            }
            return employees.InvalidEmployees;
        }

        int IUserSecurityManager.DeleteViewableBranches(string userName)
        {
            return _expressRepository.DeleteViewableBranches(userName);
        }

        List<UserCertification> IUserSecurityManager.GrantAllUserCerts(string userName)
        {
            // return List of UserCerts = List of UserCertifications 
            List<UserCerts> certList = _expressRepository.GrantAllTraining(userName);
            List<UserCertification> passbackCertList = new List<UserCertification>();
            UserCertification userCertification;

            foreach (var cert in certList)
            {
                userCertification = new UserCertification();
                userCertification.EmpTrainingID = cert.EmpTrainingID;
                userCertification.EmpCode = cert.EmpCode;
                userCertification.CourseID = cert.CourseID;
                userCertification.DatePassed = cert.DatePassed;
                userCertification.ExpirationDate = cert.ExpirationDate;
                userCertification.EnteredDate = cert.EnteredDate;
                userCertification.EnteredBy = cert.EnteredBy;

                passbackCertList.Add(userCertification);
            }

            return passbackCertList;

        }

        string IUserSecurityManager.CrossEnvironmentClone(string userName, List<ScreenSecurity> screenSecurities)
        {

            _expressRepository.DeleteAllScreenRights(userName);

            foreach (var screen in screenSecurities)
            {
                _expressRepository.UpdateScreenRight(userName, screen.Name, Convert.ToInt32(screen.AccessLevel));
            }

            return "OK";
        }
    }
}
