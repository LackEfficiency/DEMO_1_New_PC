using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    Transform parentToReturnTo = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo =this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
    }
}

