using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinaryTree : IDataStructure
{
    private static int max_children = 64;
    private long[] tree;

    public override void Init()
    {
        view.transform.Destroy_All_Children();
        ViewHandler.Instance.Change_Grid(enabled: false);
        tree = new long[max_children];

        for (int i = 0; i < max_children; i++)
        {
            tree[i] = Int64.MaxValue;
        }

        create_node(empty_data: true);
    }

    public void Add_To_Array(GameObject node)
    {
        long value = long.Parse(node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text);
        if (view.transform.childCount == 1)
        {
            tree[0] = value;
        }
        else
        {
            int position = Get_Parent_Position(node);

            if (position == -1)
            {
                return;
            }
            else
            {
                if (node.name == "Left")
                {
                    tree[2 * position + 1] = value;
                }
                else if (node.name == "Right")
                {
                    tree[2 * position + 2] = value;
                }
            }
        }
    }

    public void In_Order_Traversal()
    {
        Stack<long> st = new Stack<long>();

        int current = 0;


        st.Push(tree[current]);

        while (tree[current] < Int64.MaxValue && current < tree.Length)
        {

            while (tree[current] < Int64.MaxValue)
            {
                current = 2 * current + 1;
                st.Push(tree[current]);
            }

        }

    }

    private int Get_Parent_Position(GameObject node)
    {
        int value = int.Parse(node.GetComponentInChildren<Text>().text);
        for (int i = 0; i < max_children; i++)
        {
            if (tree[i] == value)
            {
                return i;
            }
        }

        return -1;
    }

    public void AddNode(GameObject parent)
    {
        GameObject new_node = Instantiate(node, parent.transform);

        if (parent.name.Contains("LEFT"))
        {
            new_node.transform.localPosition = new Vector3(-11f, -12f, 0);
            new_node.name = "Left";
        }
        else
        {
            new_node.transform.localPosition = new Vector3(11f, -12f, 0);
            new_node.name = "Right";
        }

        GameObject node_parent = parent.transform.parent.gameObject;

        Text parent_node = new_node.GetComponentInChildren<Text>();
        parent_node.text = node_parent.transform.Get_Child(0, 0).GetComponent<TMPro.TextMeshProUGUI>().text;


        new_node.transform.SetParent(view.transform);

        parent.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);

    }
}
