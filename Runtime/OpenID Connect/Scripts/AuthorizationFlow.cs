using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The authorization flow of the OpenID Connect procedure
/// It is recommended to use the Authorization Code flow
/// </summary>
public enum AuthorizationFlow
{
    AUTHORIZATION_CODE, IMPLICIT
}
