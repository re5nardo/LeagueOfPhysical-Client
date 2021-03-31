using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
public class ManagedCanvas : MonoBehaviour
{
    [SerializeField] private CanvasLayer layer = CanvasLayer.Contents;
    [SerializeField][Range(0, LAYER_SCOPE - 1)] private int sortingOrder;

    private const int LAYER_SCOPE = 1000;

    public UnityEvent<int> onSortingOrderChanged = new UnityEvent<int>();

    public CanvasLayer Layer { get; protected set; }
    public Canvas Canvas { get; protected set; }
    public int SortingOrder
    {
        get => Canvas.sortingOrder;
        protected set
        {
            Canvas.sortingOrder = Util.IndexOf(typeof(CanvasLayer), Layer) * LAYER_SCOPE + value;

            onSortingOrderChanged.Invoke(Canvas.sortingOrder);
        }
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

        onSortingOrderChanged.RemoveAllListeners();
    }

    protected virtual void Initialize()
    {
        Canvas = GetComponent<Canvas>();
        Layer = layer;
        SortingOrder = sortingOrder;
    }

    private void OnValidate()
    {
        if (Canvas == null)
        {
            Canvas = GetComponent<Canvas>();
        }
        Layer = layer;
        SortingOrder = sortingOrder;
    }
}
