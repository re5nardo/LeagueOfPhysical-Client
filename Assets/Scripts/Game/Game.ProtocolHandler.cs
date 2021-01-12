using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace LOP
{
    public partial class Game
    {
        #region Protocol Handler
        public void OnEnterRoom(IMessage msg)
        {
            SC_EnterRoom enterRoom = msg as SC_EnterRoom;

            MyInfo.EntityID = enterRoom.m_nEntityID;
            GameUI.EmotionExpressionSelector.SetData(0, 1, 2, 3);   //  Dummy

            Run(enterRoom.m_nCurrentTick);
        }
        #endregion
    }
}
