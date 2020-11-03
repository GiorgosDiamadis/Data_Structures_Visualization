using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GraphNode : MonoBehaviour, IPointerClickHandler
{
    private static Graphs graphs;
    private List<GraphNode> connections = null;
    public int data;
    
    private void OnEnable()
    {
        graphs = FindObjectOfType<Graphs>();
        connections = new List<GraphNode>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Graphs.on_select_node.Invoke(this);
        }
    }

    public void Add_Edge()
    {
        graphs.Add_Edge(from:this);
    }

    public void Delete()
    {
        graphs.Delete();
    }


    public void Add_Connection(GraphNode node)
    {
        connections.Add(node);

        for(int i = 0; i < connections.Count; i++)
        {
            print(connections[i].data);
        }
    }
}
