using UnityEngine;

public class Edge
{
    public GraphNode from;
    public GraphNode to;
    public GameObject obj;
    public int weight;

    public Edge(GraphNode from, GraphNode to, GameObject obj, int weight)
    {
        this.from = from;
        this.to = to;
        this.obj = obj;
        this.weight = weight;
    }
}
