using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GraphNode : MonoBehaviour, IPointerClickHandler
{
    private static Graphs graphs;
    public List<Edge> connections = null;
    public int data;

    private void OnEnable()
    {
        graphs = FindObjectOfType<Graphs>();
        connections = new List<Edge>();
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
        graphs.Add_Edge(from: this);
    }
    public void Delete()
    {
        graphs.Delete_Node();
    }

    public void dfs()
    {
        StartCoroutine(graphs.DFS(this));
    }

    public void Change_Value()
    {
        TMPro.TMP_InputField n = transform.Get_Component_In_Child<TMPro.TMP_InputField>(1,0);

        int d = int.Parse(n.text);

        data = d;
        transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();
    }
    public void bfs()
    {
        StartCoroutine(graphs.BFS(this));
    }

    public void djkstr()
    {
        GraphNode destination = null;

        TMPro.TMP_InputField n = transform.Get_Component_In_Child<TMPro.TMP_InputField>(1,5);

        int d = int.Parse(n.text);



        foreach(GraphNode g in FindObjectsOfType<GraphNode>())
        {
            if(g.data == d)
            {
                destination = g;
                break;
            }
        }


        StartCoroutine(graphs.Dijkstra(this,destination));
    }

    public void Change_Weight(GraphNode to, int data)
    {
        foreach (Edge p in connections)
        {
            if (p.to == to)
            {
                p.weight = data;
                break;
            }
        }

    }

    public void Add_Connection(Edge edge)
    {
        connections.Add(edge);
    }



    //When deleting an edge
    public void Remove_Edge(GraphNode to)
    {
        Edge p = null;

        foreach (Edge pa in connections)
        {
            if (pa.to == to)
            {
                p = pa;
            }
        }

        connections.Remove(p);


        foreach (Edge pa in to.connections)
        {
            if (pa.to == this)
            {
                p = pa;
            }
        }

        connections.Remove(p);

    }


    //When deleting a node
    public void Remove_Edges(GraphNode with)
    {
        connections.RemoveAll(p => p.from == with || p.to == with);
    }
}
