using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkModel.Mirror;

public class SC_PlayerEntityHandler
{
    public static void Handle(SC_PlayerEntity playerEntity)
    {
        SceneDataContainer.Get<MyInfo>().EntityId = playerEntity.playerEntityId;

        //  �� ���� ���ӿ��� ó�� �ؾ��� ��? (���������� �ٸ��� �����Ƿ�..)
        LOP.Game.Current.GameUI.CameraController.Target = Entities.MyCharacter.Transform;
        LOP.Game.Current.GameUI.CameraController.FollowTarget = true;
        LOP.Game.Current.GameUI.PlayerInputController.SetCharacterID(Entities.MyEntityId);
    }
}
