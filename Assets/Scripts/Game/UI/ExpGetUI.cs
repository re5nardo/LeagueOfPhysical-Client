using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpGetUI : MonoBehaviour
{
    [SerializeField] private Text text = null;

    private Transform trMine = null;
    private RectTransform rtMine = null;
    private RectTransform rtCanvas = null;

    private Vector3 screenOffset = new Vector3(0, Screen.height * 0.05f, 0);

    private void Awake()
    {
        trMine = transform;
        rtMine = GetComponent<RectTransform>();
        rtCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public void SetData(Vector3 vec3Position, string strText)
    {
        Vector3 vec3Pos = Camera.main.WorldToScreenPoint(vec3Position);
        //vec3Pos.z = 0;

        rtMine.anchoredPosition = Util.UGUI.ConvertWorldToCanvas(vec3Pos + screenOffset, rtCanvas);

        text.text = strText;
    }

    private void Update()
    {
        rtMine.anchoredPosition = Util.UGUI.ConvertWorldToCanvas(new Vector3(trMine.localPosition.x, trMine.localPosition.y + 50f * Time.deltaTime, trMine.localPosition.z), rtCanvas);
    }
}
