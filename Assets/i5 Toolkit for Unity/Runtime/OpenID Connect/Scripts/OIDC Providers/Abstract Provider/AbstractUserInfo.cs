using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Description of the user information data for the client
    /// </summary>
    public class AbstractUserInfo : IUserInfo
    {
        [SerializeField] private string name;
        [SerializeField] private string email;

        /// <summary>
        /// The username of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public string Username { get => name; }

        /// <summary>
        /// The email address of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public string Email { get => email; }

        /// <summary>
        /// A clear name of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public string FullName { get => name; }

        /// <summary>
        /// Creates a new instance of the user info with the given parameters
        /// </summary>
        /// <param name="username">The user name of the user</param>
        /// <param name="email">The email address of the user</param>
        /// <param name="fullName">The full name of the user</param>
        public UserInfo(string name, string email)
        {
            this.name = name;
            this.email = email;
        }
    }
}
