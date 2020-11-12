using UnityEngine;
using UnityEngine.EventSystems;

public class ClickActions : MonoBehaviour,IPointerClickHandler
{
    private static Graphs graphs;
    private Edge edge = null;

    private void Start()
    {
        graphs = FindObjectOfType<Graphs>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            foreach (GraphNode g in FindObjectsOfType<GraphNode>())
            {
                foreach(Edge e in g.connections)
                {
                    if(e.obj == gameObject)
                    {
                        edge = e;
                        break;
                    }
                }    
            
            }

            Graphs.on_select_edge.Invoke(edge);
        }
    }

    public void Remove()
    {
        graphs.Remove_Edge(edge);
        edge = null;

    }

    public void Add_Weight()
    {
        graphs.Add_Weight(edge);
    }
}
