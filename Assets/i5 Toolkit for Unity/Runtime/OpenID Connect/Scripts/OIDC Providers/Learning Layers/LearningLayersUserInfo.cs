using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Description of the user information data for the Learning Layers client
    /// </summary>
    public class LearningLayersUserInfo : AbstractUserInfo
    {
        [SerializeField] private string sub;
        [SerializeField] private string preferred_username;
        [SerializeField] private string given_name;
        [SerializeField] private string family_name;
        [SerializeField] private string updated_time;
        [SerializeField] private bool email_verified;


        /// <summary>
        /// The username of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public override string Username { get => preferred_username; }


        /// <summary>
        /// A clear name of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public override string FullName { get => given_name + " " + family_name; }

        /// <summary>
        /// Creates a new instance of the learning layers user info with the given parameters
        /// </summary>
        /// <param name="username">The user name of the user</param>
        /// <param name="email">The email address of the user</param>
        /// <param name="fullName">The full name of the user</param>
        public LearningLayersUserInfo(string username, string email, string fullName)
        {
            this.preferred_username = username;
            this.email = email;
            this.given_name = fullName;
        }
    }
}
