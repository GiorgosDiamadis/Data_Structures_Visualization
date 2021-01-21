using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropGraphNode : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.GetComponent<Slider>() == null)
            Graphs.node_dropped.Invoke();
    }
}

