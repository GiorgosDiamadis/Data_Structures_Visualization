using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Edge : MonoBehaviour, IPointerClickHandler
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
}
