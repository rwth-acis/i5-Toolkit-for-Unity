using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Contract specifying how user information that can be accessed from an OIDC provider 
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// The user name of the logged in user
        /// </summary>
        string Username { get; }

        /// <summary>
        /// The full name of the logged in user
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// The email address of the logged in user
        /// </summary>
        string Email { get; }
    }
}