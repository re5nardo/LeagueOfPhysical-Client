using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class ManagedCanvasManager : MonoSingleton<ManagedCanvasManager>
{
    private Dictionary<CanvasLayer, List<ManagedCanvas>> managedCanvases = new Dictionary<CanvasLayer, List<ManagedCanvas>>();

    public List<ManagedCanvas> Get(CanvasLayer canvasLayerFlag)
    {
        List<ManagedCanvas> result = new List<ManagedCanvas>();

        foreach (var canvases in managedCanvases)
        {
            if ((canvases.Key & canvasLayerFlag) != 0)
            {
                result.AddRange(canvases.Value);
            }
        }

        result.Sort((x, y) =>
        {
            return x.SortOrder.CompareTo(y.SortOrder);
        });

        return result;
    }

    public ManagedCanvas Get(CanvasLayer canvasLayerFlag, string name)
    {
        var managedCanvases = Get(canvasLayerFlag);

        return managedCanvases.Find(managedCanvas => managedCanvas.name == name);
    }

    public List<ManagedCanvas> GetAll()
    {
        return Get(CanvasLayer.Contents | CanvasLayer.Popup | CanvasLayer.System);
    }

    public ManagedCanvas GetTopMost(CanvasLayer canvasLayerFlag)
    {
        var managedCanvases = Get(canvasLayerFlag);

        return managedCanvases[managedCanvases.Count - 1];
    }

    public void Register(ManagedCanvas canvas)
    {
        if (!managedCanvases.ContainsKey(canvas.Layer))
        {
            managedCanvases.Add(canvas.Layer, new List<ManagedCanvas>());
        }

        if (managedCanvases[canvas.Layer].Contains(canvas))
        {
            Debug.LogError($"canvas already exists! canvas.name : {canvas.name}");
            return;
        }

        managedCanvases[canvas.Layer].Add(canvas);
    }

    public void Unregister(ManagedCanvas canvas)
    {
        if (!managedCanvases.ContainsKey(canvas.Layer))
        {
            Debug.LogError($"canvas layer doesn't exist! canvas.Layer : {canvas.Layer}");
            return;
        }

        if (!managedCanvases[canvas.Layer].Contains(canvas))
        {
            Debug.LogError($"canvas doesn't exist! canvas.name : {canvas.name}");
            return;
        }

        managedCanvases[canvas.Layer].Remove(canvas);
    }
}
