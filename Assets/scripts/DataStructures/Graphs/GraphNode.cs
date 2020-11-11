using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pair:IComparable<Pair>
{
    public GraphNode from;
    public GraphNode to;
    public int data;

    public Pair(GraphNode from, GraphNode to, int weight)
    {
        this.from = from;
        this.to = to;
        this.data = weight;
    }

    public int CompareTo(Pair other)
    {
        if (data < other.data)
            return -1;
        else if (data > other.data)
            return 1;
        else
            return 0;
    }
}


public class GraphNode : MonoBehaviour, IPointerClickHandler
{
    private static Graphs graphs;
    public List<Pair> connections = null;
    public int data;

    private void OnEnable()
    {
        graphs = FindObjectOfType<Graphs>();
        connections = new List<Pair>();
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
        foreach (Pair p in connections)
            print(p.to.data);

        StartCoroutine(graphs.DFS(this));
    }

    public void bfs()
    {
        StartCoroutine(graphs.BFS(this));
    }

    public void djkstr()
    {
        GraphNode destination = null;

        TMPro.TMP_InputField n = transform.GetComponentInChildren<TMPro.TMP_InputField>();

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
        foreach (Pair p in connections)
        {
            if (p.to == to)
            {
                p.data = data;
                break;
            }
        }

    }

    public void Add_Connection(GraphNode node, int weight = 1)
    {
        connections.Add(new Pair(this, node, weight));
    }

    //When deleting an edge
    public void Remove_Pair(GraphNode to)
    {
        Pair p = null;

        foreach (Pair pa in connections)
        {
            if (pa.to == to)
            {
                p = pa;
            }
        }

        connections.Remove(p);


        foreach (Pair pa in to.connections)
        {
            if (pa.to == this)
            {
                p = pa;
            }
        }

        connections.Remove(p);

    }


    //When deleting a node
    public void Remove_Pairs(GraphNode with)
    {
        connections.RemoveAll(p => p.from == with || p.to == with);
    }
}
