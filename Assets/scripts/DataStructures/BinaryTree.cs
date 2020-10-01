using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class BinaryTreeNode
{
    public BinaryTreeNode left;
    public BinaryTreeNode right;
    public GameObject scene_object;
    public long data;
    public int position;

    public BinaryTreeNode(long data,int position)
    {
        this.data = data;
        this.left = null;
        this.right = null;
        this.position = position;
    }
}


public class BinaryTree : IDataStructure
{
    private static int max_children = 1024;
    private long[] tree;
    BinaryTreeNode head;

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
        head = null;
        Create_Tree_From_Array();
        In_Order(head);
    }

    private void In_Order(BinaryTreeNode head)
    {
        Stack<BinaryTreeNode> s = new Stack<BinaryTreeNode>();
        BinaryTreeNode curr = head;

        // traverse the tree  
        while (curr != null || s.Count > 0)
        {
            while (curr != null)
            {
                s.Push(curr);
                curr = curr.left;
            }
            curr = s.Pop();
            curr = curr.right;
        }
    }

    private void Create_Tree_From_Array()
    {
        head = new BinaryTreeNode(tree[0],0);
        head.scene_object = Find_In_View(tree[0]);
        
        BinaryTreeNode current;
        Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();

        queue.Enqueue(head);
        int position = 0;

        for(int i = 0; i < tree.Length && queue.Count!=0; i++)
        {
            current = queue.Dequeue();
            position = current.position;

            if (tree[2 * position + 1] < Int64.MaxValue)
            {
                current.left = new BinaryTreeNode(tree[2 * position + 1], 2 * position + 1);
                current.left.scene_object = Find_In_View(tree[2 * position + 1]);
                queue.Enqueue(current.left);
            }

            if (tree[2 * position + 2] < Int64.MaxValue)
            {
                current.right = new BinaryTreeNode(tree[2 * position + 2], 2 * position + 2);
                current.right.scene_object = Find_In_View(tree[2 * position + 2]);
                queue.Enqueue(current.right);

            }
        }
    }

    private GameObject Find_In_View(long v)
    {
        for(int i = 0; i < view.transform.childCount; i++)
        {
            if(long.Parse(view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(i,0,0).text) == v)
            {
                return view.transform.GetChild(i).gameObject;
            }
        }
        return null;
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
        parent_node.text = node_parent.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text;


        new_node.transform.SetParent(view.transform);

        parent.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);

    }
}
