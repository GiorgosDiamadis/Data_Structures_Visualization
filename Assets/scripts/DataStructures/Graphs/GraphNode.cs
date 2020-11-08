using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GraphNode : MonoBehaviour, IPointerClickHandler
{
    private static Graphs graphs;
    public List<GraphNode> connections = null;
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
        graphs.Delete_Node();
    }

    public void dfs()
    {
        StartCoroutine(graphs.DFS(this));
    }

    public void bfs()
    {
        StartCoroutine(graphs.BFS(this));
    }

    public void djkstr()
    {
        StartCoroutine(graphs.Dijkstra(this));
    }

    public void Add_Connection(GraphNode node)
    {
        connections.Add(node);
    }
}
