using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Edge : MonoBehaviour, IPointerClickHandler,IComparable<Edge>
{
    public GraphNode from;
    public GraphNode to;
    public GameObject obj;
    public int weight;
    private static Graphs graphs;

    private void OnEnable()
    {
        graphs = FindObjectOfType<Graphs>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Graphs.on_select_edge.Invoke(this);
        }
    }
    public void Remove()
    {
        graphs.Remove_Edge(this);
    }

    public void Add_Weight()
    {
        graphs.Add_Weight(this);
    }

    public int CompareTo(Edge other)
    {
        if (weight < other.weight)
            return -1;
        else if (weight > other.weight)
            return 1;
        else
            return 0;
    }
}
