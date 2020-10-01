﻿using DG.Tweening;
using System;
using System.Collections;
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

    public BinaryTreeNode(long data, int position)
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
    [SerializeField] private GameObject traversal_prefab = null;
    static GameObject p;

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
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Create_Tree_From_Array();
        StartCoroutine(In_Order_Cor());
    }

    public void Post_Order_Traversal()
    {
        head = null;
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Create_Tree_From_Array();
        StartCoroutine(Post_Order_Cor());
    }

    private IEnumerator Post_Order_Cor()
    {

        Load_Pseudocode_Nodes("Post Order");
        yield return new WaitForSeconds(speed);

        BinaryTreeNode curr = null;
        BinaryTreeNode prev = null;

        Stack<BinaryTreeNode> S = new Stack<BinaryTreeNode>();
        curr = head;

        S.Push(curr);

        while (S.Count != 0)
        {
            curr = S.Peek();
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return new WaitForSeconds(speed);
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            if (prev == null || prev.left == curr ||
                                        prev.right == curr)
            {

                if (curr.left != null)
                {
                    S.Push(curr.left);

                }
                else if (curr.right != null)
                {
                    S.Push(curr.right);
                }
                else
                {
                    S.Pop();
                    curr.scene_object.transform.Set_Child_Active(true, 1);

                    Create_Pseudocode_Nodes(curr);

                    yield return new WaitForSeconds(speed);
                }
            }
            else if (curr.left == prev)
            {
                if (curr.right != null)
                {
                    S.Push(curr.right);

                }
                else
                {
                    S.Pop();
                    curr.scene_object.transform.Set_Child_Active(true, 1);

                    Create_Pseudocode_Nodes(curr);

                    yield return new WaitForSeconds(speed);

                }
            }
            else if (curr.right == prev)
            {
                S.Pop();
                curr.scene_object.transform.Set_Child_Active(true, 1);

                Create_Pseudocode_Nodes(curr);

                yield return new WaitForSeconds(speed);

            }

            prev = curr;
        }
        yield return new WaitForSeconds(speed);
        for (int i = 0; i < view.transform.childCount; i++)
        {
            view.transform.Set_Child_Active(false, i, 1);
        }
    }

    public void Pre_Order_Traversal()
    {
        head = null;
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Create_Tree_From_Array();
        StartCoroutine(Pre_Order_Cor());
    }

    private IEnumerator Pre_Order_Cor()
    {
        Load_Pseudocode_Nodes("Pre Order");
        yield return new WaitForSeconds(speed);

        Stack<BinaryTreeNode> nodeStack = new Stack<BinaryTreeNode>();
        BinaryTreeNode curr = null;

        nodeStack.Push(head);

        while (nodeStack.Count > 0)
        {

            curr = nodeStack.Peek();
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return new WaitForSeconds(speed);
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;


            nodeStack.Pop();

            curr.scene_object.transform.Set_Child_Active(true, 1);

            Create_Pseudocode_Nodes(curr);

            yield return new WaitForSeconds(speed);

            if (curr.right != null)
            {
                nodeStack.Push(curr.right);
            }
            if (curr.left != null)
            {
                nodeStack.Push(curr.left);
            }
        }

        yield return new WaitForSeconds(speed);
        for (int i = 0; i < view.transform.childCount; i++)
        {
            view.transform.Set_Child_Active(false, i, 1);
        }
    }

    public void Level_Order_Traversal()
    {
        head = null;
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Create_Tree_From_Array();
        StartCoroutine(Level_Order_Cor());
    }
    private IEnumerator Level_Order_Cor()
    {
        Load_Pseudocode_Nodes("Level Order");
        yield return new WaitForSeconds(speed);

        Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
        queue.Enqueue(head);

        while (queue.Count != 0)
        {
            BinaryTreeNode curr = queue.Dequeue();

            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return new WaitForSeconds(speed);
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            curr.scene_object.transform.Set_Child_Active(true, 1);
            Create_Pseudocode_Nodes(curr);
            yield return new WaitForSeconds(speed);

            if (curr.left != null)
            {
                queue.Enqueue(curr.left);
            }
            if (curr.right != null)
            {
                queue.Enqueue(curr.right);
            }
        }

        yield return new WaitForSeconds(speed);
        for (int i = 0; i < view.transform.childCount; i++)
        {
            view.transform.Set_Child_Active(false, i, 1);
        }

    }

    private void Load_Pseudocode_Nodes(string trav)
    {
        if (p != null)
            Destroy(p);

        p = Resources.Load("prefabs/pseudocode/Traversal/Traversal") as GameObject;
        p.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0).text = $"{trav} Traversal:";
        p = Instantiate(p, pseudocode_panel.transform);
        p.GetComponent<RectTransform>().DOScale(1f, speed);
    }



    private IEnumerator In_Order_Cor()
    {

        Load_Pseudocode_Nodes("In Order");
        yield return new WaitForSeconds(speed);

        Stack<BinaryTreeNode> s = new Stack<BinaryTreeNode>();
        BinaryTreeNode curr = head;

        curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
        yield return new WaitForSeconds(speed);
        curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;


        // traverse the tree  
        while (curr != null || s.Count > 0)
        {
            while (curr != null)
            {
                s.Push(curr);
                curr = curr.left;

                if (curr != null && curr.left!=null)
                {
                    curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                    yield return new WaitForSeconds(speed);
                    curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                }

            }

            curr = s.Pop();

            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return new WaitForSeconds(speed);
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            curr.scene_object.transform.Set_Child_Active(true, 1);

            Create_Pseudocode_Nodes(curr);

            yield return new WaitForSeconds(speed);

            curr = curr.right;

            if (curr != null && curr.right!=null && curr.left!=null)
            {
                curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                yield return new WaitForSeconds(speed);
                curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
            }

        }

        yield return new WaitForSeconds(speed);
        for (int i = 0; i < view.transform.childCount; i++)
        {
            view.transform.Set_Child_Active(false, i, 1);
        }

    }

    private static void Create_Pseudocode_Nodes(BinaryTreeNode curr)
    {
        GameObject n = Instantiate(new GameObject());

        n.AddComponent<Image>();
        n.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);

        GameObject u = Instantiate(curr.scene_object.transform.GetChild(0).gameObject);
        GameObject y = Instantiate(curr.scene_object.transform.GetChild(1).gameObject);

        u.transform.SetParent(n.transform);
        y.transform.SetParent(n.transform);

        n.transform.SetParent(pseudocode_panel.transform.Get_Child(0, 1));
        n.transform.localScale = new Vector3(1f,1f,1f);
    }

    /*
     *Create a tree structure given it's array representation.
     *If parent node is at position i,then it's left child is located at 2*i+1 and it's right child at 2*i+2
     *
     *A Queue data structure is necessary because the values in the array are located exactly like a level order traversal
     */
    private void Create_Tree_From_Array()
    {
        head = new BinaryTreeNode(tree[0], 0);
        head.scene_object = Find_In_View(tree[0]);

        BinaryTreeNode current;
        Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();

        queue.Enqueue(head);
        int position = 0;

        for (int i = 0; i < tree.Length && queue.Count != 0; i++)
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
        for (int i = 0; i < view.transform.childCount; i++)
        {
            if (long.Parse(view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(i, 0, 0).text) == v)
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
