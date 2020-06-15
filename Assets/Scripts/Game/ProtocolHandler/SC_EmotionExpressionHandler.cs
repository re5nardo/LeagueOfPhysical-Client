using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SC_EmotionExpressionHandler : IHandler<IPhotonEventMessage>
{
    public void Handle(IPhotonEventMessage msg)
    {
        SC_EmotionExpression emotionExpression = msg as SC_EmotionExpression;

        IEntity entity = EntityManager.Instance.GetEntity(emotionExpression.m_nEntityID);
        if (entity == null)
        {
            return;
        }

        GameObject goEmotionExpressionViewer = ResourcePool.Instance.GetResource(Define.ResourcePath.UI.EMOTION_EXPRESSION_VIEWER, LOP.Game.Current.GameUI.GetTopMostCanvas().transform);
        EmotionExpressionViewer emotionExpressionViewer = goEmotionExpressionViewer.GetComponent<EmotionExpressionViewer>();
        emotionExpressionViewer.SetData(entity, emotionExpression.m_nEmotionExpressionID);

        goEmotionExpressionViewer.AddComponent<ResourceReturnAgent>().m_fDelayTime = 2f;
    }
}
