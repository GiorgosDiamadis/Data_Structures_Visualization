using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVLTree : BinaryTree
{
    public override void Init()
    {
        base.Init();
        head = new BinaryTreeNode(tree[0]);
        head.scene_object = Find_In_View(tree[0]);
    }

    public IEnumerator add(long data)
    {

        yield return new WaitForSeconds(.1f);
        head = insert(head, data);
        Update_Array();


        Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
        BinaryTreeNode current = null;
        queue.Enqueue(head);

        while (queue.Count != 0)
        {
            current = queue.Dequeue();

            if(current != head)
                current.scene_object.transform.localPosition = positions[current.position];

            if (current.left != null)
                queue.Enqueue(current.left);

            if (current.right != null)
                queue.Enqueue(current.right);
        }

        GameHandler.Instance.is_running = false;
    }

    private void Update_Array()
    {
        Create_Array_From_Tree();

        Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
        BinaryTreeNode current = null;
        queue.Enqueue(head);

        while (queue.Count != 0)
        {
            current = queue.Dequeue();

            current.position = Get_Array_Position(current.scene_object);


            if (current.left != null)
                queue.Enqueue(current.left);

            if (current.right != null)
                queue.Enqueue(current.right);
        }
    }

    BinaryTreeNode insert(BinaryTreeNode node, long key)
    {

        if (node == null)
        {
            BinaryTreeNode new_node = new BinaryTreeNode(key);


            new_node.scene_object = Instantiate(node_prefab, view.transform);
            new_node.scene_object.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = key.ToString();

            return new_node;
        }

        if (key < node.data)
        {
            node.left = insert(node.left, key);

        }
        else if (key > node.data)
        {
            node.right = insert(node.right, key);

        }
        else
        {
            return node;
        }

        node.height = 1 + max(height(node.left),
                            height(node.right));

        int balance = getBalance(node);

        if (balance > 1 && key < node.left.data)
            return rightRotate(node);

        if (balance < -1 && key > node.right.data)
            return leftRotate(node);

        if (balance > 1 && key > node.left.data)
        {
            node.left = leftRotate(node.left);
            return rightRotate(node);
        }

        if (balance < -1 && key < node.right.data)
        {
            node.right = rightRotate(node.right);
            return leftRotate(node);
        }

        return node;
    }

    void preOrder(BinaryTreeNode node)
    {
        if (node != null)
        {
            print("data:" + node.data +" " + node.scene_object);
            preOrder(node.left);
            preOrder(node.right);
        }
    }

    private int max(int a, int b)
    {
        return (a > b) ? a : b;
    }

   private int getBalance(BinaryTreeNode node)
    {
        if (node == null)
            return 0;

        return height(node.left) - height(node.right);
    }

    private int height(BinaryTreeNode node)
    {
        if (node == null)
            return 0;

        return node.height;
    }

    BinaryTreeNode rightRotate(BinaryTreeNode y)
    {
        BinaryTreeNode x = y.left;
        BinaryTreeNode T2 = x.right;


        if(x.right!=null && y != null)
            x.right.position = y.position;
        x.right = y;

        if(y.left!=null && T2 != null)
            y.left.position = T2.position;

        y.left = T2;

        y.height = max(height(y.left),
                    height(y.right)) + 1;
        x.height = max(height(x.left),
                    height(x.right)) + 1;

        return x;
    }

    BinaryTreeNode leftRotate(BinaryTreeNode x)
    {
        BinaryTreeNode y = x.right;
        BinaryTreeNode T2 = y.left;

        if(y.left!=null && x != null)
            y.left.position = x.position;

        y.left = x;

        if(x.right!=null && T2!=null)
            x.right.position = T2.position;
        
        x.right = T2;

        x.height = max(height(x.left),
                    height(x.right)) + 1;
        y.height = max(height(y.left),
                    height(y.right)) + 1;

        return y;
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
