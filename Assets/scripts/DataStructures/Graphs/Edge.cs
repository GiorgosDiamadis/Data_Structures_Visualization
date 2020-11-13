using UnityEngine;

public class Edge
{
    public GraphNode from;
    public GraphNode to;
    public GameObject gameObject;
    public int weight;

    public Edge(GraphNode from, GraphNode to, GameObject obj, int weight)
    {
        this.from = from;
        this.to = to;
        this.gameObject = obj;
        this.weight = weight;
    }
}
