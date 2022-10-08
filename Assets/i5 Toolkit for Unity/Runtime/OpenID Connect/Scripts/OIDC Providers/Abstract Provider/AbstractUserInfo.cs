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
        [SerializeField] protected string name;
        [SerializeField] protected string email;

        /// <summary>
        /// The username of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public virtual string Username { get => name; }

        /// <summary>
        /// The email address of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public virtual string Email { get => email; }

        /// <summary>
        /// A clear name of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public virtual string FullName { get => name; }
    }
}
