using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLTree : BinaryTree
{
    public override void Init()
    {
        base.Init();
        head = new BinaryTreeNode(tree[0], Find_In_View(tree[0]));
    }

    public IEnumerator add(long data)
    {

        BinaryTreeNode current = head;
        Stack<BinaryTreeNode> parents = new Stack<BinaryTreeNode>();
        BinaryTreeNode new_node = new BinaryTreeNode(data);
        bool exists = false;
        int pos = 0;

        while (current != null)
        {
            if (data < current.Get_Data())
            {
                pos = 2 * pos + 1;
                current = current.Get_Left();
            }
            else if (data > current.Get_Data())
            {
                pos = 2 * pos + 2;
                current = current.Get_Right();
            }
            else if (data == current.Get_Data())
            {
                pos = 0;
                print(pos);
                break;
            }
        }

        
        if (pos < 31)
        {
            current = head;
            while (current != null)
            {
                if (data < current.Get_Data())
                {
                    parents.Push(current);
                    current = current.Get_Left();
                    new_node.Change_Child_Type(BinaryTreeNode.Child_Type.Left);
                }
                else if (data > current.Get_Data())
                {
                    parents.Push(current);
                    current = current.Get_Right();
                    new_node.Change_Child_Type(BinaryTreeNode.Child_Type.Right);

                }
                else if (data == current.Get_Data())
                {
                    UIHandler.Instance.show_message("Key already exists");
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                BinaryTreeNode new_node_parent = parents.Peek();
                new_node.Change_Parent_To(new_parent: new_node_parent);

                if (new_node.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
                    new_node_parent.Change_Left_To(new_left: new_node);
                else
                    new_node_parent.Change_Right_To(new_right: new_node);


                new_node.Set_Scene_Object(Instantiate(node_prefab, view.transform), data);

                int position = 0;
                int parent_position = -1;
                Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
                Queue<int> position_queue = new Queue<int>();
                Queue<int> parent_queue = new Queue<int>();

                queue.Enqueue(head);
                position_queue.Enqueue(position);
                parent_queue.Enqueue(parent_position);

                while (queue.Count != 0)
                {
                    current = queue.Dequeue();
                    position = position_queue.Dequeue();
                    parent_position = parent_queue.Dequeue();


                    current.Get_GameObject().transform.localPosition = positions[position];

                    if (parent_position != -1)
                    {
                        if (current.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
                        {
                            current.Get_Parent().Get_GameObject().transform.Get_Child(2).localScale = scales[parent_position];
                            current.Get_Parent().Get_GameObject().transform.Get_Child(2).eulerAngles = new Vector3(0, 0, rotations_left[parent_position]);
                        }
                        else
                        {
                            current.Get_Parent().Get_GameObject().transform.Get_Child(3).localScale = scales[parent_position];
                            current.Get_Parent().Get_GameObject().transform.Get_Child(3).eulerAngles = new Vector3(0, 0, rotations_right[parent_position]);
                        }
                    }

                    if (position >= 15)
                    {
                        current.Get_GameObject().transform.Set_Child_Active(active: false, 2);
                        current.Get_GameObject().transform.Set_Child_Active(active: false, 3);
                    }

                    if (current.Get_Left() != null)
                    {
                        queue.Enqueue(current.Get_Left());
                        parent_queue.Enqueue(position);
                        position_queue.Enqueue(2 * position + 1);
                    }
                    if (current.Get_Right() != null)
                    {
                        queue.Enqueue(current.Get_Right());
                        parent_queue.Enqueue(position);
                        position_queue.Enqueue(2 * position + 2);
                    }
                }
                
                
                new_node.Change_Height();
                Rebalance(parents);


                yield return null;
            }

        }
        else
        {
            UIHandler.Instance.show_message("Cant have more than 5 levels");
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

        else if (v.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
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
                v.Get_Left().Change_Height();
                v.Get_Right().Change_Height();
                v.Change_Height();
            }
            v = v.Get_Parent();
        }
    }

    private BinaryTreeNode Reconstruct(BinaryTreeNode v, BinaryTreeNode w, BinaryTreeNode u)
    {
        if (w.Get_Child_Type() == BinaryTreeNode.Child_Type.Left &&
           u.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
        {
            if (v != head)
            {
                if (v.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
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
        else if (w.Get_Child_Type() == BinaryTreeNode.Child_Type.Right &&
                 u.Get_Child_Type() == BinaryTreeNode.Child_Type.Right)
        {
            if (v != head)
            {
                if (v.Get_Child_Type() == BinaryTreeNode.Child_Type.Right)
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
        else if (u.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
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
                if (v.Get_Child_Type() == BinaryTreeNode.Child_Type.Right)
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
                if (v.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
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
