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
    public class LearningLayersUserInfo : IUserInfo
    {
        [SerializeField] private string sub;
        [SerializeField] private string name;
        [SerializeField] private string preferred_username;
        [SerializeField] private string given_name;
        [SerializeField] private string family_name;
        [SerializeField] private string updated_time;
        [SerializeField] private string email;
        [SerializeField] private bool email_verfied;

        /// <summary>
        /// The username of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public string Username { get => preferred_username; }

        /// <summary>
        /// The email address of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public string Email { get => email; }

        /// <summary>
        /// A clear name of the user
        /// This is a mapping based on the available user data of the OIDC provider
        /// </summary>
        public string FullName { get => given_name + " " + family_name; }

        public LearningLayersUserInfo(string username, string email, string fullName)
        {
            this.preferred_username = username;
            this.email = email;
            this.given_name = fullName;
        }
    }
}
