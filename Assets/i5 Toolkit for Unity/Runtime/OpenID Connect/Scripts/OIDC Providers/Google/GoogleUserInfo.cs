using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Description of the user information data for the Google client
    /// </summary>
    public class GoogleUserInfo : AbstractUserInfo
    {
        [SerializeField] private string sub;
        [SerializeField] private string given_name;
        [SerializeField] private string family_name;
        [SerializeField] private bool email_verified;
        [SerializeField] private string picture;


        /// <summary>
        /// A clear name of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public override string FullName { get => given_name + " " + family_name; }

        /// <summary>
        /// An identifier for the user, unique among all Google accounts and never reused.
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public override string Username { get => sub; }

        /// <summary>
        /// Creates a new instance of the learning layers user info with the given parameters
        /// </summary>
        /// <param name="username">The user name of the user</param>
        /// <param name="email">The email address of the user</param>
        /// <param name="fullName">The full name of the user</param>
        public GoogleUserInfo(string username, string email, string fullName)
        {
            this.name = username;
            this.email = email;
            this.given_name = fullName;
        }
    }
}

