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

        Debug.Log($"���� ���� {subGameReadyNotice.timeBeforeStart:N0}�� �� �Դϴ�.");
    }
}
