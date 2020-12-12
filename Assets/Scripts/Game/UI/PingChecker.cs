using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;
using UnityEngine.UI;

public class PingChecker : MonoBehaviour
{
    [SerializeField] private Text ping = null;

    private bool response = false;
    private DateTime lastReqTime = DateTime.Now;
    private List<double> RTTs = new List<double>();

    private void Start()
    {
        RoomNetwork.Instance.onMessage += OnMessage;

        StartCoroutine(PingCoroutine());
    }

    private void OnDestroy()
    {
        if (RoomNetwork.HasInstance())
        {
            RoomNetwork.Instance.onMessage -= OnMessage;
        }
    }

    #region Message Handler
    private void OnMessage(IMessage msg, object[] objects)
    {
        if (msg is SC_Ping)
        {
            OnSC_Ping(msg as SC_Ping, (int)objects[0]);
        }
    }

    private void OnSC_Ping(SC_Ping ping, int nSenderID)
    {
        response = true;

        double rtt = (DateTime.Now - lastReqTime).TotalMilliseconds;

        RTTs.Add(rtt);

        if (RTTs.Count > 20)
        {
            RTTs.RemoveAt(0);
        }
    }
    #endregion

    private IEnumerator PingCoroutine()
    {
        while (true)
        {
            response = false;
            lastReqTime = DateTime.Now;

            RoomNetwork.Instance.Send(new CS_Ping(), PhotonNetwork.masterClient.ID, bInstant: true);

            yield return new WaitUntil(() => response);

            ping.text = string.Format("{0:0}", RTTs[RTTs.Count - 1]);

            ping.color = RTTs[RTTs.Count - 1] > 100 ? Color.red : RTTs[RTTs.Count - 1] > 60 ? Color.yellow : Color.green;

            yield return new WaitForSeconds(1);
        }
    }
}
