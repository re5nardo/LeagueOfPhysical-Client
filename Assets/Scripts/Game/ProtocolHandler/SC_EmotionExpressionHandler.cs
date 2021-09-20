using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using NetworkModel.Mirror;

public class SC_EmotionExpressionHandler
{
    public static void Handle(SC_EmotionExpression emotionExpression)
    {
        IEntity entity = Entities.Get(emotionExpression.entityId);
        if (entity == null)
        {
            return;
        }

        GameObject goEmotionExpressionViewer = ResourcePool.Instance.GetResource(Define.ResourcePath.UI.EMOTION_EXPRESSION_VIEWER, LOP.Game.Current.GameUI.GetTopMostCanvas().transform);
        EmotionExpressionViewer emotionExpressionViewer = goEmotionExpressionViewer.GetComponent<EmotionExpressionViewer>();
        emotionExpressionViewer.SetData(entity, emotionExpression.emotionExpressionId);

        goEmotionExpressionViewer.AddComponent<ResourceReturnAgent>().m_fDelayTime = 2f;
    }
}
