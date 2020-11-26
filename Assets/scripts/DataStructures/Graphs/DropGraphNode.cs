using UnityEngine;
using UnityEngine.EventSystems;

namespace Graphs
{
    public class DropGraphNode: MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            Graphs.node_dropped.Invoke();
        }
    }
}
