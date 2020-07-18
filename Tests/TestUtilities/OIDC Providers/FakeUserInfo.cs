using i5.Toolkit.Core.OpenIDConnectClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    public class FakeUserInfo : IUserInfo
    {
        public const string username = "Test User";
        public const string email = "test@user.com";
        public const string fullname = "Full Name";

        public string Username
        {
            get
            {
                return username;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
        }

        public string FullName
        {
            get
            {
                return fullname;
            }
        }
    }
}