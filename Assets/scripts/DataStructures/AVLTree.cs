using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AVLTree : BinaryTree
{
    public override void Init()
    {
        base.Init();
        head = new BinaryTreeNode(tree[0],0);
        head.scene_object = Find_In_View(head.data);
    }

    public IEnumerator add(long data)
    {

        yield return new WaitForSeconds(.1f);
        head = insert(head, data,0);
        preOrder(head);

        //int new_node_position = 0;
        //int parent_position = -1;
        //bool is_left_child = false;
        //GameObject current = null;

        //while (new_node_position < tree.Length)
        //{
        //    if (tree[new_node_position] < Int64.MaxValue)
        //    {

        //        if (data < tree[new_node_position])
        //        {
        //            parent_position = new_node_position;
        //            new_node_position = new_node_position * 2 + 1;

        //            current = Find_In_View(tree[parent_position]);

        //            current.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
        //            yield return new WaitForSeconds(speed);
        //            current.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

        //            is_left_child = true;
        //        }
        //        else
        //        {
        //            parent_position = new_node_position;
        //            new_node_position = new_node_position * 2 + 2;

        //            current = Find_In_View(tree[parent_position]);

        //            current.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
        //            yield return new WaitForSeconds(speed);
        //            current.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

        //            is_left_child = false;
        //        }
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}

        //yield return null;

        //if(new_node_position < 31)
        //{
        //    tree[new_node_position] = data;

        //    GameObject new_node = Instantiate(node, view.transform);
        //    new_node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();

        //    new_node.transform.localPosition = positions[new_node_position];
        //    GameObject parent_node = Find_In_View(tree[parent_position]);

        //    if (is_left_child)
        //    {
        //        parent_node.transform.Get_Child(2).localScale = scales[parent_position];
        //        parent_node.transform.Get_Child(2).eulerAngles = new Vector3(0, 0, rotations_left[parent_position]);
        //    }
        //    else
        //    {
        //        parent_node.transform.Get_Child(3).localScale = scales[parent_position];
        //        parent_node.transform.Get_Child(3).eulerAngles = new Vector3(0, 0, rotations_right[parent_position]);
        //    }

        //    if (new_node_position >= 15)
        //    {
        //        new_node.transform.Set_Child_Active(active: false, 2);
        //        new_node.transform.Set_Child_Active(active: false, 3);

        //    }

        //    new_node.transform.Set_Child_Active(true, 1);
        //    yield return new WaitForSeconds(speed);
        //    new_node.transform.Set_Child_Active(false, 1);

        //    Create_Tree_From_Array();
        //}

        GameHandler.Instance.is_running = false;
    }


    BinaryTreeNode insert(BinaryTreeNode node, long key,int position)
    {

        if (node == null)
        {
            BinaryTreeNode n = new BinaryTreeNode(key, position);
            return n;
        }

        if (key < node.data)
        {
            node.left = insert(node.left, key,2*position+1);

        }
        else if (key > node.data)
        {
            node.right = insert(node.right, key,2*position+2);

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
            print("data:" + node.data + " position:" + node.position);
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
