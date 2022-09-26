using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkModel.Mirror;

public class SC_SubGameReadyNoticeHandler
{
    public static void Handle(SC_SubGameReadyNotice subGameReadyNotice)
    {
        if (subGameReadyNotice.timeBeforeStart <= 4)
        {
            GameLoadingView.Hide();
        }

        Debug.Log($"게임 시작 {subGameReadyNotice.timeBeforeStart:N0}초 전 입니다.");
    }
}
