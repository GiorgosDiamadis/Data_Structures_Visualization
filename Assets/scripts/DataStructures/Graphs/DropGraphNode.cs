using UnityEngine;
using UnityEngine.EventSystems;


    public class DropGraphNode: MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            Graphs.node_dropped.Invoke();
        }
    }

