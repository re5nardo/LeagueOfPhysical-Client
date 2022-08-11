using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Mirror;

public class PingChecker : MonoBehaviour
{
    [SerializeField] private Text ping = null;

    private void Update()
    {
        if (!NetworkClient.ready) return;

        ping.text = $"{Math.Round(NetworkTime.rtt * 1000)}";

        ping.color = NetworkTime.rtt > 0.1 ? Color.red : NetworkTime.rtt > 0.06 ? Color.yellow : Color.green;
    }
}
