using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleTooltip : MonoBehaviour,IEndDragHandler,IBeginDragHandler
{
    bool b = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        b = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        b = false;

    }

    void Update()
    {
        if(b == true)
            GameHandler.Instance.tooltip.SetActive(false);
        else
            GameHandler.Instance.tooltip.SetActive(true);

    }
}
