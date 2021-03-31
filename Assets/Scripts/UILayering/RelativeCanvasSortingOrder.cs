using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class RelativeCanvasSortingOrder : MonoBehaviour
{
    [SerializeField] private ManagedCanvas target = null;
    
    private Canvas myCanvas = null;

    private int relativeValue = 0;
    public int RelativeValue
    {
        get => relativeValue;
        set
        {
            relativeValue = value;
            UpdateSortingOrder();
        }
    }

    private void Awake()
    {
        myCanvas = GetComponent<Canvas>();

        target.onSortingOrderChanged.AddListener(OnSortingOrderChanged);

        UpdateSortingOrder();
    }

    private void OnDestroy()
    {
        target.onSortingOrderChanged.RemoveListener(OnSortingOrderChanged);
    }

    private void OnSortingOrderChanged(int sortingOrder)
    {
        UpdateSortingOrder();
    }

    private void UpdateSortingOrder()
    {
        myCanvas.sortingOrder = target.SortingOrder + RelativeValue;
    }
}
