using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLTree : BinaryTree
{
    public override void Init()
    {
        base.Init();
        //node.transform.Get_Component_In_Child<ClickAddNode>(5).enabled = false;
        //node.transform.Get_Component_In_Child<ClickAddNode>(6).enabled = false;

    }

    public IEnumerator add(long data)
    {
        int new_node_position = 0;
        int parent_position = -1;
        bool is_left_child = false;

        while (new_node_position < tree.Length)
        {
            if (tree[new_node_position] < Int64.MaxValue)
            {
                print(tree[new_node_position]);
                if (data < tree[new_node_position])
                {
                    parent_position = new_node_position;
                    new_node_position = new_node_position * 2 + 1;
                    is_left_child = true;
                }
                else
                {
                    parent_position = new_node_position;
                    new_node_position = new_node_position * 2 + 2;
                }
            }
            else
            {
                break;
            }
        }

        yield return null;

        tree[new_node_position] = data;

        GameObject new_node = Instantiate(node, view.transform);
        new_node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();

        new_node.transform.localPosition = positions[new_node_position];
        GameObject parent_node = Find_In_View(tree[parent_position]);

        if (is_left_child)
        {
            parent_node.transform.Get_Child(3).localScale = scales[parent_position];
            parent_node.transform.Get_Child(3).eulerAngles = new Vector3(0, 0, rotations_left[parent_position]);
        }
        else
        {
            parent_node.transform.Get_Child(4).localScale = scales[parent_position];
            parent_node.transform.Get_Child(4).eulerAngles = new Vector3(0, 0, rotations_right[parent_position]);
        }

        GameHandler.Instance.is_running = false;
    }

    public IEnumerator delete(long data)
    {
        yield return null;
    }

    public IEnumerator search(long data)
    {
        yield return null;
    }
}
