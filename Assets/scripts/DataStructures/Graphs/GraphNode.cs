using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pair
{
    public GraphNode from;
    public GraphNode to;
    public int weight;

    public Pair(GraphNode from, GraphNode to, int weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
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
        graphs.Add_Edge(from:this);
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
        StartCoroutine(graphs.Dijkstra(this));
    }

    public void Add_Connection(GraphNode node,int weight = 1)
    {
        connections.Add(new Pair(this,node,weight));
    }

    //When deleting an edge
    public void Remove_Pair(GraphNode to)
    {
        Pair p = null;

        foreach(Pair pa in connections)
        {
            if(pa.to == to)
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
