﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleLinkedList : IList
{
    [SerializeField] private bool is_init = false;
    private GameObject new_node = null;
    private TMPro.TextMeshProUGUI new_node_data = null;
    private List<string> list;

    public override void add_node(int data)
    {
        create_arrow();
        create_node(data);
        list.Add(data.ToString());
    }

    public override void init_list()
    {
        if (is_init)
            return;

        list = new List<string>();

        for (int i = 0; i < init_number; i++)
        {
            create_node();

            if (i < init_number - 1)
            {
                create_arrow();
            }
        }

        is_init = true;
    }

    private void create_arrow()
    {
        Instantiate(arrow, view.transform);
    }

    private void create_node(int? data = null)
    {
        new_node = Instantiate(node, view.transform);
        new_node_data = new_node.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        
        if(!data.HasValue)
            new_node_data.text = (Random.Range(-100, 100)).ToString();
        else
            new_node_data.text = (data.Value).ToString();

        list.Add(new_node_data.text);
    }
}
