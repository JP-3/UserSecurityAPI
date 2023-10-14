using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using UserSecurity.API.DataModels;
using UserSecurity.API.Interfaces.Repositories;
using UserSecurity.API.Interfaces.Database;
using System.Configuration;

namespace UserSecurity.API.Repositories
{
    public class ExpressRepository : IExpressRepository
    {
        private readonly ISqlConnection sqlConnection;
        private readonly ISqlCommand sqlCommand;
        private readonly string _expressConnection = ConfigurationManager.ConnectionStrings["ExpressDB"].ConnectionString;

        public ExpressRepository(ISqlConnection connection, ISqlCommand command)
        {
            sqlConnection = connection;
            sqlCommand = command;
        }

        List<Screen> IExpressRepository.ReadScreens()
        {
            List<Screen> screens = new List<Screen>();

            using (var connection = sqlConnection.GetNewInstance(_expressConnection))
            {
                using (var command = sqlCommand.GetNewInstance("dbo.GetScreensWithSecurity", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (ISqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Screen screen = new Screen();
                            screen.ScreenID = int.Parse(reader["ScreenID"].ToString());
                            screen.ScreenName = reader["ScreenName"].ToString();
                            screen.AccessLevelDefault = int.Parse(reader["AccessLevelDefault"].ToString());
                            screen.Sponsor = reader["Sponsor"].ToString();

                            screen.SponsorApprovalLevel = 0;
                            if (screen.Sponsor != string.Empty && reader["SponsorApprovalLevel"].ToString() != string.Empty)
                            {
                                screen.SponsorApprovalLevel = Byte.Parse(reader["SponsorApprovalLevel"].ToString());
                            }
                            screens.Add(screen);
                        }
                        return screens;
                    }
                }
            }
        }

        Employee IExpressRepository.ReadEmployee(string userName)
        {
            Employee employee = new Employee();
            using (var connection = sqlConnection.GetNewInstance(_expressConnection))
            {
                using (var command = sqlCommand.GetNewInstance($"dbo.GetEmployee N'{userName}'", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (ISqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employee.FirstName = reader["FirstName"].ToString().Trim();
                            employee.LastName = reader["LastName"].ToString().Trim();
                            employee.Manager = reader["Manager"].ToString().Trim();
                            employee.Department = reader["Department"].ToString().Trim();
                        }
                    }
                }
            }

            using (var connection = sqlConnection.GetNewInstance(_expressConnection))
            {
                using (var command = sqlCommand.GetNewInstance($"dbo.GetEmployeeBranch N'{userName}'", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (ISqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employee.Branch = reader["BranchCode"].ToString().Trim();
                        }
                    }
                }
            }
            return employee;
        }

        List<UserScreenSecurity> IExpressRepository.ReadUserScreenSecurities(string userName)
        {
            List<UserScreenSecurity> userScreenSecurity = new List<UserScreenSecurity>();
            using (var connection = sqlConnection.GetNewInstance(_expressConnection))
            {
                using (var command = sqlCommand.GetNewInstance($"execute [dbo].[GetEmployeeSecurityRights] @EmpCode = N'{userName}'", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (ISqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserScreenSecurity userScreen = new UserScreenSecurity();
                            userScreen.ScreenID = int.Parse(reader["ScreenID"].ToString());
                            userScreen.AccessLevel = int.Parse(reader["AccessLevel"].ToString());
                            userScreenSecurity.Add(userScreen);
                        }
                        return userScreenSecurity;
                    }
                }
            }
        }

        List<ScreenAccessLevel> IExpressRepository.ReadScreenAccessLevels()
        {
            List<ScreenAccessLevel> accessLevels = new List<ScreenAccessLevel>();
            ScreenAccessLevel level;

            using (var connection = sqlConnection.GetNewInstance(_expressConnection))
            {
                using (var command = sqlCommand.GetNewInstance("execute [dbo].[GetAllScreenLevels]", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (ISqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            level = new ScreenAccessLevel();
                            level.ScreenID = int.Parse(reader["ScreenID"].ToString());
                            level.AccessLevel = int.Parse(reader["AccessLevel"].ToString());
                            level.LevelDescription = reader["LevelDescription"].ToString();
                            accessLevels.Add(level);
                        }
                        return accessLevels;
                    }
                }
            }
        }

        List<UserBranchView> IExpressRepository.ReadUserViewableBranches(string userName)
        {
            List<UserBranchView> branches = new List<UserBranchView>();
            UserBranchView userBranch;

            using (var connection = sqlConnection.GetNewInstance(_expressConnection))
            {
                using (var command = sqlCommand.GetNewInstance($"execute [dbo].[GetEmployeeBranchView] @EmpCode = N'{userName}'", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (ISqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userBranch = new UserBranchView();
                            userBranch.Branch = reader["BranchCode"].ToString();
                            userBranch.Active = bool.Parse(reader["Active"].ToString());
                            userBranch.UpdatedBy = reader["UpdatedBy"].ToString().ToUpper().Trim();
                            userBranch.UpdatedDate = DateTime.Parse(reader["UpdatedDate"].ToString());
                            userBranch.EditMoney = bool.Parse(reader["EditMoney"].ToString());
                            branches.Add(userBranch);
                        }
                        return branches;
                    }
                }
            }
        }

        int IExpressRepository.MoveBranch(string userName, string branch, string branchManager, string department)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@empcode", userName));
            parameters.Add(new SqlParameter("@branchcode", branch));
            parameters.Add(new SqlParameter("@manager", branchManager));
            parameters.Add(new SqlParameter("@department", department));
            return ExecuteStoredProcedure("asd_updateemployee", parameters);
        }

        int IExpressRepository.CloneTraining(string userNameA, string userNameB)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@FromEmpCode", userNameA));
            parameters.Add(new SqlParameter("@ToEmpCode", userNameB));
            return ExecuteStoredProcedure("CloneUserCerts", parameters);
        }

        List<UserCerts> IExpressRepository.GrantAllTraining(string userName)
        {
            List<UserCerts> certs = new List<UserCerts>();
            UserCerts userCert;

            using (var connection = sqlConnection.GetNewInstance(_expressConnection))
            {
                using (var command = sqlCommand.GetNewInstance($"execute [dbo].[GrantAllUserCerts] @ToEmpCode = N'{userName}'", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (ISqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userCert = new UserCerts();
                            userCert.EmpTrainingID = int.Parse(reader["EmpTrainingID"].ToString());
                            userCert.EmpCode = reader["EmpCode"].ToString().ToUpper().Trim();
                            userCert.CourseID = int.Parse(reader["CourseID"].ToString());
                            userCert.DatePassed = DateTime.Parse(reader["DatePassed"].ToString());
                            userCert.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());
                            userCert.EnteredDate = DateTime.Parse(reader["EnteredDate"].ToString());
                            userCert.EnteredBy = reader["EnteredBy"].ToString().ToUpper().Trim();
                            certs.Add(userCert);
                        }
                        return certs;
                    }
                }
            }
        }

        int IExpressRepository.CloneUserScreenRights(string userNameA, string userNameB)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@FromEmpCode", userNameA));
            parameters.Add(new SqlParameter("@ToEmpCode", userNameB));
            return ExecuteStoredProcedure("CloneUserSecurityScreenRights", parameters);
        }

        int IExpressRepository.UpdateViewableBranch(string userName, string branchCode, bool active, string updatedBy, bool editMoney)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EmpCode", userName));
            parameters.Add(new SqlParameter("@BranchCode", branchCode));
            parameters.Add(new SqlParameter("@Active", active));
            parameters.Add(new SqlParameter("@UpdatedBy", updatedBy));
            parameters.Add(new SqlParameter("@EditMoney", editMoney));
            return ExecuteStoredProcedure("UpdateEmployeeBranchView", parameters);
        }


        int IExpressRepository.UpdateScreenSecurity(string userName, int screenID, int accessLevel)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EmpCode", userName));
            parameters.Add(new SqlParameter("@ScreenID", screenID));
            parameters.Add(new SqlParameter("@AccessLevel", accessLevel));
            return ExecuteStoredProcedure("UpdateUserSecurity", parameters);
        }

        int IExpressRepository.DeleteViewableBranches(string userName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EmpCode", userName));
            return ExecuteStoredProcedure("DeleteEmployeeBranchView", parameters);
        }

        /// <summary>
        /// Clears the last row in the table that stores data for employee branch moves, after 255 you get an error as you are over the limit
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        int IExpressRepository.PurgeEmployeeBranchMoves(string userName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EmpCode", userName));
            parameters.Add(new SqlParameter("@MinMoveNum", "254"));
            return ExecuteStoredProcedure("ASD_PurgeEmpMoves", parameters);
        }

        private int ExecuteStoredProcedure(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            using (var connection = sqlConnection.GetNewInstance(_expressConnection))
            {
                using (var command = sqlCommand.GetNewInstance(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter parameter in sqlParameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        int IExpressRepository.UpdateScreenRight(string empCode, string name, int accessLevel)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EmpCode", empCode));
            parameters.Add(new SqlParameter("@ScreenName", name));
            parameters.Add(new SqlParameter("@accessLevel", accessLevel));
            return ExecuteStoredProcedure("UpdateUserSecurityByName", parameters);
        }

        int IExpressRepository.DeleteAllScreenRights(string empCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EmpCode", empCode));
            return ExecuteStoredProcedure("Support_DeleteUserSecurityAll", parameters);
        }
    }
}
