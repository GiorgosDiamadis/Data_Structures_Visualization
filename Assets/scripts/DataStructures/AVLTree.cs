using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLTree : BinaryTree
{
    public override void Init()
    {
        base.Init();
        head = new BinaryTreeNode(tree[0],BinaryTreeNode.Child_Type.Root);
        head.scene_object = Find_In_View(tree[0]);
    }

    public IEnumerator add(long data)
    {

        yield return new WaitForSeconds(.1f);


        BinaryTreeNode current = head;
        Stack<BinaryTreeNode> parents = new Stack<BinaryTreeNode>();
        BinaryTreeNode new_node = new BinaryTreeNode(data);

        while (current != null)
        {
            if(data < current.data)
            {
                parents.Push(current);
                current = current.left;
                new_node.child_type = BinaryTreeNode.Child_Type.Left;
            }
            else if (data > current.data)
            {
                parents.Push(current);
                current = current.right;
                new_node.child_type = BinaryTreeNode.Child_Type.Right;
            }
        }

        BinaryTreeNode new_node_parent = parents.Peek();

        if (new_node.child_type == BinaryTreeNode.Child_Type.Left)
            new_node_parent.left = new_node;
        else
            new_node_parent.right = new_node;

        new_node.height = 1 + max(height(new_node.left) , height(new_node.right));



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
            print("data:" + node.data + " " + node.scene_object);
            preOrder(node.left);
            preOrder(node.right);
        }
    }


    #region Visual
    private void Update_Tree_Visual()
    {
        Update_Array();

        Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
        BinaryTreeNode current = null;
        queue.Enqueue(head);

        while (queue.Count != 0)
        {
            current = queue.Dequeue();

            current.scene_object.transform.localPosition = positions[current.position];


            if (current.has_not_children())
            {
                current.scene_object.transform.Get_Child(2).localScale = Vector3.one;
                current.scene_object.transform.Get_Child(2).eulerAngles = new Vector3(0, 0, 70);

                current.scene_object.transform.Get_Child(3).localScale = Vector3.one;
                current.scene_object.transform.Get_Child(3).eulerAngles = new Vector3(0, 0, 100);
            }
            else if (!current.has_left_child())
            {
                current.scene_object.transform.Get_Child(2).localScale = Vector3.one;
                current.scene_object.transform.Get_Child(2).eulerAngles = new Vector3(0, 0, 70);
            }
            else if (!current.has_right_child())
            {
                current.scene_object.transform.Get_Child(3).localScale = Vector3.one;
                current.scene_object.transform.Get_Child(3).eulerAngles = new Vector3(0, 0, 100);
            }

            if (current.parent > -1)
            {

                GameObject parent = Find_In_View(tree[current.parent]);
                int parent_position = Get_Array_Position(parent);


                if (current.scene_object.name == "Left")
                {
                    parent.transform.Get_Child(2).localScale = scales[parent_position];
                    parent.transform.Get_Child(2).eulerAngles = new Vector3(0, 0, rotations_left[parent_position]);
                }
                else
                {
                    parent.transform.Get_Child(3).localScale = scales[parent_position];
                    parent.transform.Get_Child(3).eulerAngles = new Vector3(0, 0, rotations_right[parent_position]);
                }


            }

            if (current.left != null)
            {
                queue.Enqueue(current.left);
            }

            if (current.right != null)
            {
                queue.Enqueue(current.right);
            }
        }
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
            {
                queue.Enqueue(current.left);
                current.left.parent = current.position;
                current.left.scene_object.name = "Left";
            }

            if (current.right != null)
            {
                queue.Enqueue(current.right);
                current.right.parent = current.position;
                current.right.scene_object.name = "Right";

            }
        }
    }
    #endregion
    #region rotations
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


        x.right = y;

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


        y.left = x;
        x.right = T2;

        x.height = max(height(x.left),
                    height(x.right)) + 1;
        y.height = max(height(y.left),
                    height(y.right)) + 1;

        return y;
    }
    #endregion
    
}
