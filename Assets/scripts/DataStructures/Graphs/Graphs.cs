using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graphs : IDataStructure
{
    private Outline drop_area;
    public static int node_count = 0;
    [SerializeField] private GameObject drag_area;
    public static GameObject selected_graph;
    public override void Init()
    {
        view.transform.Destroy_All_Children();

        view.GetComponent<DropGraphNodeOrArrow>().enabled = true;
        drop_area = view.GetComponent<Outline>();
        drag_area.SetActive(true);
        drop_area.enabled = true;

    }

    public void Delete()
    {
        selected_graph.Destroy_Object();
        node_count--;

        Update_Matrix();
    }

    private void Update_Matrix()
    {

    }
}
