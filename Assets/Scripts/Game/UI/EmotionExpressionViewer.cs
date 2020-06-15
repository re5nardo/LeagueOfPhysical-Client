using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;

public class EmotionExpressionViewer : MonoBehaviour
{
    [SerializeField] private RawImage image = null;

    private IEntity m_targetEntity = null;
    private Transform tfMine = null;

    private RectTransform rectTransformParent = null;
    private RectTransform RectTransformParent { get { return rectTransformParent ?? (rectTransformParent = transform.parent?.GetComponent<RectTransform>()); } }

    private void Awake()
    {
        tfMine = transform;
    }

    public void SetData(IEntity target, int emotionExpressionID)
    {
        m_targetEntity = target;

        MasterData.EmotionExpression master = MasterDataManager.Instance.GetMasterData<MasterData.EmotionExpression>(emotionExpressionID);

        image.texture = Resources.Load<Texture>(master.ResID);
    }

    private void LateUpdate()
    {
        if (RectTransformParent == null || m_targetEntity == null)
            return;

        var center = Util.UGUI.ConvertScreenToLocalPoint(RectTransformParent, Camera.main.WorldToScreenPoint(m_targetEntity.Position));
        tfMine.localPosition = new Vector3(center.x, center.y + 120, center.z);
    }
}
