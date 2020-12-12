using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class RoomPunBehaviour : PunBehaviour
{
    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.LogError("[OnMasterClientSwitched] Error!");
    }
}
