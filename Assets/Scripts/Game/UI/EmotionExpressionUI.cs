using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;

public class EmotionExpressionUI : MonoBehaviour
{
    [SerializeField] private RawImage image = null;

    private IEntity m_targetEntity = null;
    private RectTransform rtMine = null;
    private RectTransform rtCanvas = null;

    private Vector3 m_vec3ScreenOffset = new Vector3(0, Screen.height * 0.2f, 0);

    private void Awake()
    {
        rtMine = GetComponent<RectTransform>();
        rtCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public void SetData(IEntity target, int nEmtionExpressionID)
    {
        m_targetEntity = target;

        MasterData.EmotionExpression master = MasterDataManager.Instance.GetMasterData<MasterData.EmotionExpression>(nEmtionExpressionID);

        image.texture = Resources.Load<Texture>(master.ResID);
    }

    private void LateUpdate()
    {
        if (m_targetEntity != null)
        {
            Vector3 vec3Pos = Camera.main.WorldToScreenPoint(m_targetEntity.Position);
            //vec3Pos.z = 0;

            rtMine.anchoredPosition = Util.UGUI.ConvertWorldToCanvas(vec3Pos + m_vec3ScreenOffset, rtCanvas);
        }
    }
}
