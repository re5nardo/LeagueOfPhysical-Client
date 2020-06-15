using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputCircleDisplay : MonoBehaviour
{
	[SerializeField] private Image outerCircle = null;
	[SerializeField] private Image innerCircle = null;

	private Transform tfMine = null;
    private RectTransform rtMine = null;

    private RectTransform rectTransformParent = null;
    private RectTransform RectTransformParent { get { return rectTransformParent ?? (rectTransformParent = transform.parent?.GetComponent<RectTransform>()); } }

    private float maxRadius = 0f;

	private void Awake()
	{
        tfMine = transform;
        rtMine = GetComponent<RectTransform>();

        maxRadius = (outerCircle.rectTransform.rect.height - innerCircle.rectTransform.rect.height) / 2;
	}

    public void Show(Vector2 vec2ScreenPosition)
    {
        gameObject.SetActive(true);

        tfMine.localPosition = Util.UGUI.ConvertScreenToLocalPoint(RectTransformParent, vec2ScreenPosition);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        innerCircle.transform.localPosition = Vector3.zero;
    }

	public void OnTouchHold(Vector2 vec2ScreenPosition)
    {
        var dest = Util.UGUI.ConvertScreenToLocalPoint(rtMine, vec2ScreenPosition);

        if (dest.magnitude < maxRadius)
        {
            innerCircle.transform.localPosition = dest;
        }
        else
        {
            innerCircle.transform.localPosition = dest.normalized * maxRadius;
        }
    }
}
