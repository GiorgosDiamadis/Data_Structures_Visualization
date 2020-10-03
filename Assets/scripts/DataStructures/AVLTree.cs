using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLTree : BinaryTree
{
    public override void Init()
    {
        base.Init();
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
                    is_left_child = false;
                }
            }
            else
            {
                break;
            }
        }

        yield return null;

        if(new_node_position < 31)
        {
            tree[new_node_position] = data;

            GameObject new_node = Instantiate(node, view.transform);
            new_node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();

            new_node.transform.localPosition = positions[new_node_position];
            GameObject parent_node = Find_In_View(tree[parent_position]);

            if (is_left_child)
            {
                parent_node.transform.Get_Child(2).localScale = scales[parent_position];
                parent_node.transform.Get_Child(2).eulerAngles = new Vector3(0, 0, rotations_left[parent_position]);
            }
            else
            {
                parent_node.transform.Get_Child(3).localScale = scales[parent_position];
                parent_node.transform.Get_Child(3).eulerAngles = new Vector3(0, 0, rotations_right[parent_position]);
            }

            if (new_node_position >= 15)
            {
                new_node.transform.Set_Child_Active(active: false, 2);
                new_node.transform.Set_Child_Active(active: false, 3);

            }
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
