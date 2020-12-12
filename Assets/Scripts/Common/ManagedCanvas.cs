using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ManagedCanvas : MonoBehaviour
{
    [SerializeField] private CanvasLayer layer = CanvasLayer.Contents;
    [SerializeField][Range(0, LAYER_SCOPE - 1)] private int sortOrder;

    private const int LAYER_SCOPE = 1000;

    public CanvasLayer Layer { get; protected set; }
    public Canvas Canvas { get; protected set; }
    public int SortOrder
    {
        get => Canvas.sortingOrder;
        protected set => Canvas.sortingOrder = Util.IndexOf(typeof(CanvasLayer), Layer) * LAYER_SCOPE + value;
    }

    public void Register()
    {
        ManagedCanvasManager.Instance.Register(this);
    }

    public void Unregister()
    {
        if (ManagedCanvasManager.HasInstance())
        {
            ManagedCanvasManager.Instance.Unregister(this);
        }
    }

    private void Awake()
    {
        Initialize();

        Register();
    }

    private void OnDestroy()
    {
        Unregister();
    }

    protected virtual void Initialize()
    {
        Canvas = GetComponent<Canvas>();
        Layer = layer;
        SortOrder = sortOrder;
    }

    private void OnValidate()
    {
        if (Canvas == null)
        {
            Canvas = GetComponent<Canvas>();
        }
        Layer = layer;
        SortOrder = sortOrder;
    }
}
