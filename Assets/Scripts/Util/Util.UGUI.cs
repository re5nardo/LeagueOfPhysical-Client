using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.IO.Compression;

public partial class Util
{
    public class UGUI
    {
        public static Vector2 ConvertWorldToCanvas(Vector3 worldPosition, RectTransform canvasRect)
        {
            //then you calculate the position of the UI element
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5f to get the correct position.
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
            Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            //UI_Element.anchoredPosition = WorldObject_ScreenPosition;

            return WorldObject_ScreenPosition;
        }

        //  Canvas.renderMode must be RenderMode.ScreenSpaceOverlay
        public static Vector3 ConvertScreenToLocalPoint(RectTransform rtParent, Vector2 screenPoint)
        {
            Vector2 localPoint = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rtParent, screenPoint, null, out localPoint);

            return new Vector3(localPoint.x, localPoint.y);
        }
    }
}