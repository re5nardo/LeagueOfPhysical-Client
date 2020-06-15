using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingItem: MonoBehaviour
{
    [SerializeField] private Text text = null;

    private Transform tfMine = null;

    private void Awake()
    {
        tfMine = transform;
    }

    public void SetData(Vector2 screenPosition, string text)
    {
        tfMine.localPosition = Util.UGUI.ConvertScreenToLocalPoint(transform.parent.GetComponent<RectTransform>(), screenPosition);
        this.text.text = text;
    }

    public void SetData(Vector2 screenPosition, string strText, Color color)
    {
        SetData(screenPosition, strText);

        text.color = color;
    }

    private void LateUpdate()
    {
        tfMine.localPosition = new Vector3(tfMine.localPosition.x, tfMine.localPosition.y + 1, tfMine.localPosition.z);
    }
}
