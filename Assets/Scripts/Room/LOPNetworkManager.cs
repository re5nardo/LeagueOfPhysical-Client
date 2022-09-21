using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class LOPNetworkManager : NetworkManager
{
    #region Start & Stop Callbacks
    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();

        Debug.Log("[OnStartClient]");
    }

    /// <summary>
    /// This is called when a client is stopped.
    /// </summary>
    public override void OnStopClient()
    {
        base.OnStopClient();

        Debug.Log("[OnStopClient]");
    }
    #endregion

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (NetworkClient.isConnected)
        {
            StopClient();
        }
        ResetStatics();
    }
}
