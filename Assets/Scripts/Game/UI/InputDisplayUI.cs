using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputDisplayUI : MonoBehaviour
{
	[SerializeField] Image m_texOuterCircle = null;
	[SerializeField] Image m_texInnerCircle = null;

	private Transform m_trMine = null;
	private Transform m_trInnerCircle = null;
    private RectTransform rtMine = null;
    private RectTransform rtCanvas = null;

    private float m_fMaxRadius = 0f;

	private void Awake()
	{
		m_trMine = transform;
		m_trInnerCircle = m_texInnerCircle.transform;
        rtMine = GetComponent<RectTransform>();
        rtCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        m_fMaxRadius = (m_texOuterCircle.rectTransform.sizeDelta.x - m_texInnerCircle.rectTransform.sizeDelta.x) / rtCanvas.rect.height;
	}

    public void Show(Vector3 vec3Position)
    {
        gameObject.SetActive(true);

        //m_trMine.position = vec3Position;
        //m_trInnerCircle.position = vec3Position;

        rtMine.anchoredPosition = Util.UGUI.ConvertWorldToCanvas(vec3Position, rtCanvas);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

	public void OnTouchHold(Vector3 vec3Position)
    {
		Vector3 vec3Direction = vec3Position - m_trMine.position;

		if(vec3Direction.magnitude < m_fMaxRadius)
		{
			m_trInnerCircle.position = vec3Position;
		}
		else
		{
			m_trInnerCircle.position = m_trMine.position + vec3Direction.normalized * m_fMaxRadius;
		}
    }
}