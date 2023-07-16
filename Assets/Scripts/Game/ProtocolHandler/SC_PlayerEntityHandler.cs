using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkModel.Mirror;

public class SC_PlayerEntityHandler
{
    public static void Handle(SC_PlayerEntity playerEntity)
    {
        SceneDataContainer.Get<MyInfo>().EntityId = playerEntity.playerEntityId;

        //  각 서브 게임에서 처리 해야할 듯? (컨텐츠마다 다를수 있으므로..)
        LOP.Game.Current.GameUI.CameraController.Target = Entities.MyCharacter.Transform;
        LOP.Game.Current.GameUI.CameraController.FollowTarget = true;
        LOP.Game.Current.GameUI.PlayerInputController.SetCharacterID(Entities.MyEntityId);
    }
}
