using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Description of the user information data for the Git Hub client
    /// </summary>
    public class GitHubUserInfo : AbstractUserInfo
    {
        /// <summary>
        /// Creates a new instance of the GitHub user info with the given parameters
        /// </summary>
        /// <param name="loginName">The user name of the user</param>
        /// <param name="email">The email address of the user</param>
        public GitHubUserInfo(string loginName, string email)
        {
            this.name = loginName;
            this.email = email;
        }
    }
}
