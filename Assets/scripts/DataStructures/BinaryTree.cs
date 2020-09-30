﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BinaryTree : IDataStructure
{
    public override void Init()
    {
        view.transform.Destroy_All_Children();
        ViewHandler.Instance.Change_Grid(enabled: false);
        create_node(empty_data:true);
    }

    public void AddNode(GameObject parent)
    {
        GameObject new_node =  Instantiate(node, parent.transform);

        if(parent.name.Contains("LEFT"))
            new_node.transform.localPosition = new Vector3(-11f, -12f, 0);
        else
            new_node.transform.localPosition = new Vector3(11f, -12f, 0);

        new_node.transform.SetParent(view.transform);

        parent.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);

    }
}
