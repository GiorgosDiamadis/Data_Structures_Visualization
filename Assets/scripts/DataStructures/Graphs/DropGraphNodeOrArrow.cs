using UnityEngine;
using UnityEngine.EventSystems;

public class DropGraphNodeOrArrow : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Graphs.node_dropped.Invoke();
    }
}
