using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public interface IUserInfo
    {
        string Username { get; }

        string Email { get; }
    }
}