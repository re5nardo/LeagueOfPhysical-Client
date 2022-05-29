using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseCode
{
    //  Success
    public const int SUCCESS = 0;

    //  Match
    public const int INVALID_TO_MATCHMAKING = 300;
    public const int ALREADY_IN_GAME = 301;

    #region User
    public const int USER_NOT_EXIST = 30000;
    #endregion

    public const int UNKNOWN_ERROR = 99999;
}
