﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOP
{
    public partial class Game
    {
        #region Protocol Handler
        public void OnEnterRoom(IPhotonEventMessage msg)
        {
            SC_EnterRoom enterRoom = msg as SC_EnterRoom;

            MyInfo.EntityID = enterRoom.m_nEntityID;
            GameUI.EmotionExpressionSelectUI.SetData(0, 1, 2, 3);   //  Dummy

            Run(enterRoom.m_nCurrentTick);
        }
        #endregion
    }
}
