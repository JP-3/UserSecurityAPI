using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Auth;

namespace UserSecurity.API.ServiceDefinition
{
    public static class UserProfile
    {
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns>Username from the Session email</returns>
        public static string GetUserName(this IAuthSession session)
        {
            return session?.UserAuthId?.Replace("@Test.com", "") ?? string.Empty;
        }

        /// <summary>
        /// Attempts to retreive the userId from the Session first, then from the Request
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="requestUserId">The request user identifier.</param>
        /// <returns>UserId</returns>
        // TODO: Once the Auth0 logic is fixed, remove this overload and fix the service methods
        public static string GetUserName(this IAuthSession session, string requestUserId)
        {
            string userId = session?.GetUserName();
            // First check the session userId
            if (string.IsNullOrEmpty(userId))
            {
                // If the seesion UserId was blank, then try to assign the request UserId
                if (!string.IsNullOrEmpty(requestUserId))
                {
                    userId = requestUserId;
                }
                else
                {
                    // TODO: Should this throw an error, or should the locking logic throw the error
                }
            }
            return userId;
        }
    }
}
