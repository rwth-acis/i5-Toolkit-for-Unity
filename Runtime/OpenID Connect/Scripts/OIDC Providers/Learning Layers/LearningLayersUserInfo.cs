using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
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

        public string Username { get => preferred_username; }

        public string Email { get => email; }

        public string FullName { get => given_name; }
    }
}
