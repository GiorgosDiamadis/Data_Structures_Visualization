using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLTree : BinaryTree
{
    public override void Init()
    {
        base.Init();
        head = new BinaryTreeNode(tree[0], BinaryTreeNode.Child_Type.Root);
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
            if (data < current.Get_Data())
            {
                parents.Push(current);
                current = current.Get_Left();
                new_node.child_type = BinaryTreeNode.Child_Type.Left;
            }
            else if (data > current.Get_Data())
            {
                parents.Push(current);
                current = current.Get_Right();
                new_node.child_type = BinaryTreeNode.Child_Type.Right;
            }
        }

        BinaryTreeNode new_node_parent = parents.Peek();
        new_node.parent = new_node_parent;

        if (new_node.child_type == BinaryTreeNode.Child_Type.Left)
            new_node_parent.Change_Left_To(new_left: new_node);
        else
            new_node_parent.Change_Right_To(new_right: new_node);

        new_node.Change_Height();
        Rebalance(parents);

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

        //if (node == null)
        //{
        //    BinaryTreeNode new_node = new BinaryTreeNode(key);

        //    new_node.scene_object = Instantiate(node_prefab, view.transform);
        //    new_node.scene_object.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = key.ToString();

        //    return new_node;
        //}

        //if (key < node.Get_Data())
        //{
        //    node.left = insert(node.left, key);

        //}
        //else if (key > node.Get_Data())
        //{
        //    node.right = insert(node.right, key);

        //}
        //else
        //{
        //    return node;
        //}

        //node.Change_Height();

        //int balance = node.Get_Balance();

        //if (balance > 1 && key < node.left.Get_Data())
        //    return rightRotate(node);

        //if (balance < -1 && key > node.right.Get_Data())
        //    return leftRotate(node);

        //if (balance > 1 && key > node.left.Get_Data())
        //{
        //    node.left = leftRotate(node.left);
        //    return rightRotate(node);
        //}

        //if (balance < -1 && key < node.right.Get_Data())
        //{
        //    node.right = rightRotate(node.right);
        //    return leftRotate(node);
        //}

        return node;
    }

    void preOrder(BinaryTreeNode node)
    {
        if (node != null)
        {
            print("data:" + node.Get_Data() + " " + node.scene_object);
            preOrder(node.Get_Left());
            preOrder(node.Get_Right());
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


            if (current.Has_A_Child() == false)
            {
                current.scene_object.transform.Get_Child(2).localScale = Vector3.one;
                current.scene_object.transform.Get_Child(2).eulerAngles = new Vector3(0, 0, 70);

                current.scene_object.transform.Get_Child(3).localScale = Vector3.one;
                current.scene_object.transform.Get_Child(3).eulerAngles = new Vector3(0, 0, 100);
            }
            else if (!current.Has_Left_Child())
            {
                current.scene_object.transform.Get_Child(2).localScale = Vector3.one;
                current.scene_object.transform.Get_Child(2).eulerAngles = new Vector3(0, 0, 70);
            }
            else if (!current.Has_Right_Child())
            {
                current.scene_object.transform.Get_Child(3).localScale = Vector3.one;
                current.scene_object.transform.Get_Child(3).eulerAngles = new Vector3(0, 0, 100);
            }

            if (current.parent_position > -1)
            {

                GameObject parent = Find_In_View(tree[current.parent_position]);
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

            if (current.Get_Left() != null)
            {
                queue.Enqueue(current.Get_Left());
            }

            if (current.Get_Right() != null)
            {
                queue.Enqueue(current.Get_Right());
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


            if (current.Get_Left() != null)
            {
                queue.Enqueue(current.Get_Left());
                current.Get_Left().parent_position = current.position;
                current.Get_Left().scene_object.name = "Left";
            }

            if (current.Get_Right() != null)
            {
                queue.Enqueue(current.Get_Right());
                current.Get_Right().parent_position = current.position;
                current.Get_Right().scene_object.name = "Right";

            }
        }
    }
    #endregion
    #region Rotations

    private BinaryTreeNode Rebalance_Son(BinaryTreeNode v)
    {
        if (v == null)
        {
            return null;
        }

        if (v.Left_Height() > v.Right_Height())
        {
            return v.Get_Left();
        }

        else if (v.Left_Height() < v.Right_Height())
        {
            return v.Get_Right();
        }

        else if (v.child_type == BinaryTreeNode.Child_Type.Left)
        {
            return v.Get_Left();
        }
        else
        {
            return v.Get_Right();
        }
    }

    private void Rebalance(Stack<BinaryTreeNode> parents)
    {
        while (parents.Count != 0)
        {
            BinaryTreeNode current_parent = parents.Pop();
            BinaryTreeNode v = current_parent;
            BinaryTreeNode w;
            BinaryTreeNode u;

            current_parent.Change_Height();

            if (!current_parent.Is_Balanced())
            {
                print("Rotate at " + current_parent.Get_Data());

                v = current_parent;
                w = Rebalance_Son(v);
                u = Rebalance_Son(w);

                v = Reconstruct(v, w, u);
                preOrder(head);
                v.Get_Left().Change_Height();
                v.Get_Right().Change_Height();
                v.Change_Height();
            }
            v = v.Get_Parent();
        }
    }

    private BinaryTreeNode Reconstruct(BinaryTreeNode v, BinaryTreeNode w, BinaryTreeNode u)
    {
        if (w.child_type == BinaryTreeNode.Child_Type.Left &&
           u.child_type == BinaryTreeNode.Child_Type.Left)
        {
            if (v != head)
            {
                if (v.child_type == BinaryTreeNode.Child_Type.Left)
                {
                    v.Get_Parent().Change_Left_To(new_left: w);
                    w.Change_Child_Type(BinaryTreeNode.Child_Type.Left);
                }
                else
                {
                    v.Get_Parent().Change_Right_To(new_right: w);
                    w.Change_Child_Type(BinaryTreeNode.Child_Type.Right);

                }

                w.Change_Parent_To(new_parent: v.Get_Parent());
            }

            v.Change_Left_To(new_left: w.Get_Right());

            if (w.Get_Right() != null)
            {
                w.Get_Right().Change_Child_Type(BinaryTreeNode.Child_Type.Left);
                w.Get_Right().Change_Parent_To(new_parent: v);
            }

            w.Change_Right_To(new_right: v);
            v.Change_Child_Type(BinaryTreeNode.Child_Type.Right);

            v.Change_Parent_To(new_parent: w);

            if (v == head)
            {
                head = w;
                w.Change_Parent_To(new_parent: null);
            }

            return w;
        }
        else if (w.child_type == BinaryTreeNode.Child_Type.Right &&
                 u.child_type == BinaryTreeNode.Child_Type.Right)
        {
            if (v != head)
            {
                if (v.child_type == BinaryTreeNode.Child_Type.Right)
                {
                    v.Get_Parent().Change_Right_To(new_right: w);
                    w.Change_Child_Type(BinaryTreeNode.Child_Type.Right);
                }
                else
                {
                    v.Get_Parent().Change_Left_To(new_left: w);
                    w.Change_Child_Type(BinaryTreeNode.Child_Type.Left);
                }

                w.Change_Parent_To(new_parent: v.Get_Parent());

            }

            v.Change_Right_To(new_right: w.Get_Left());

            if (w.Get_Left() != null)
            {
                w.Get_Left().Change_Child_Type(BinaryTreeNode.Child_Type.Right);
                w.Get_Left().Change_Parent_To(new_parent: v);
            }

            w.Change_Left_To(new_left: v);
            v.Change_Child_Type(BinaryTreeNode.Child_Type.Left);

            v.Change_Parent_To(new_parent: w);

            if (v == head)
            {
                head = w;
                w.Change_Parent_To(new_parent: null);
            }
            return w;
        }
        else if (u.child_type == BinaryTreeNode.Child_Type.Left)
        {

            v.Change_Right_To(new_right: u.Get_Left());

            if (u.Get_Left() != null)
            {
                u.Get_Left().Change_Child_Type(BinaryTreeNode.Child_Type.Right);
                u.Get_Left().Change_Parent_To(new_parent: v);

            }

            w.Change_Left_To(new_left: u.Get_Right());

            if (u.Get_Right() != null)
            {
                u.Get_Right().Change_Child_Type(BinaryTreeNode.Child_Type.Left);
                u.Get_Right().Change_Parent_To(new_parent: w);
            }

            if (v != head)
            {
                if (v.child_type == BinaryTreeNode.Child_Type.Right)
                {
                    v.Get_Parent().Change_Right_To(new_right: u);
                    u.Change_Child_Type(BinaryTreeNode.Child_Type.Right);
                }
                else
                {
                    v.Get_Parent().Change_Left_To(new_left: u);
                    u.Change_Child_Type(BinaryTreeNode.Child_Type.Left);
                }
                u.Change_Parent_To(new_parent: v.Get_Parent());
            }

            v.Change_Parent_To(new_parent: u);
            w.Change_Parent_To(new_parent: u);

            u.Change_Left_To(new_left: v);
            v.Change_Child_Type(BinaryTreeNode.Child_Type.Left);

            u.Change_Right_To(new_right: w);
            w.Change_Child_Type(BinaryTreeNode.Child_Type.Right);

            if (v == head)
            {
                head = u;
                u.Change_Parent_To(new_parent: null);
            }

            return u;

        }
        else
        {
            v.Change_Left_To(new_left: u.Get_Right());
            if (u.Get_Right() != null)
            {
                u.Get_Right().Change_Child_Type(BinaryTreeNode.Child_Type.Left);
                u.Get_Right().Change_Parent_To(new_parent: v);

            }

            w.Change_Right_To(new_right: u.Get_Left());
            if (u.Get_Left() != null)
            {
                u.Get_Left().Change_Child_Type(BinaryTreeNode.Child_Type.Right);
                u.Get_Left().Change_Parent_To(new_parent: w);

            }

            if (v != head)
            {
                if (v.child_type == BinaryTreeNode.Child_Type.Left)
                {
                    v.Get_Parent().Change_Left_To(new_left: u);
                    u.Change_Child_Type(BinaryTreeNode.Child_Type.Left);
                }
                else
                {

                    v.Get_Parent().Change_Right_To(new_right: u);
                    u.Change_Child_Type(BinaryTreeNode.Child_Type.Right);
                }
                u.Change_Parent_To(new_parent: v.Get_Parent());
            }


            v.Change_Parent_To(new_parent: u);
            w.Change_Parent_To(new_parent: u);
            u.Change_Left_To(new_left: w);
            w.Change_Child_Type(BinaryTreeNode.Child_Type.Left);
            u.Change_Right_To(new_right: v);
            v.Change_Child_Type(BinaryTreeNode.Child_Type.Right);
            if (v == head)
            {
                head = u;
                u.Change_Parent_To(new_parent: null);
            }

            return u;

        }

    }

    #endregion
}
