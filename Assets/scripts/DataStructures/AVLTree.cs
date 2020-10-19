using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVLTree : BinaryTree
{
    [SerializeField] private Material green = null;
    [SerializeField] private Material blue = null;
    [SerializeField] private Material red = null;
    public override void Init()
    {
        base.Init();
        head = new BinaryTreeNode(tree[0], Find_In_View(tree[0]));
    }

    #region Tree Operations
    public IEnumerator add(long data)
    {
        UIHandler.Instance.close_message();
        Load_Pseudocode("add");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, is_open: true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, is_open: false);


        if (head == null)
        {
            highlight_pseudocode(1, is_open: true);
            head = new BinaryTreeNode(data);
            head.Set_Scene_Object(Instantiate(node_prefab, view.transform), data);
            GameHandler.Instance.is_running = false;
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, is_open: false);
        }
        else
        {
            BinaryTreeNode current = head;
            Stack<BinaryTreeNode> parents = new Stack<BinaryTreeNode>();
            BinaryTreeNode new_node = new BinaryTreeNode(data);
            bool exists = false;

            int pos = Check_Can_Add(data);

            if (pos < 31)
            {
                current = head;

                highlight_pseudocode(2, is_open: true);
                head.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                yield return new WaitForSeconds(speed);
                highlight_pseudocode(2, is_open: false);
                head.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

                highlight_pseudocode(3, is_open: true);
                yield return new WaitForSeconds(speed);
                highlight_pseudocode(3, is_open: false);


                while (current != null)
                {

                    if (data < current.Get_Data())
                    {

                        highlight_pseudocode(4, is_open: true);
                        yield return new WaitForSeconds(speed);
                        highlight_pseudocode(4, is_open: false);

           

                        parents.Push(current);
                        current = current.Get_Left();
                        highlight_pseudocode(5, is_open: true);
                        if(current!=null)
                            current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                        yield return new WaitForSeconds(speed);
                        if(current!=null)
                            current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                        highlight_pseudocode(5, is_open: false);
                        new_node.Change_Child_Type(BinaryTreeNode.Child_Type.Left);

                        highlight_pseudocode(3, is_open: true);
                        yield return new WaitForSeconds(speed);
                        highlight_pseudocode(3, is_open: false);
                    }
                    else if (data > current.Get_Data())
                    {

                        highlight_pseudocode(4, is_open: true);
                        yield return new WaitForSeconds(speed);
                        highlight_pseudocode(4, is_open: false);


                        highlight_pseudocode(6, is_open: true);
                        yield return new WaitForSeconds(speed);
                        highlight_pseudocode(6, is_open: false);

                      

                        parents.Push(current);
                        current = current.Get_Right();
                        new_node.Change_Child_Type(BinaryTreeNode.Child_Type.Right);

                        highlight_pseudocode(7, is_open: true);
                        if(current!=null)
                            current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                        yield return new WaitForSeconds(speed);
                        if(current!=null)
                            current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                        highlight_pseudocode(7, is_open: false);

                        highlight_pseudocode(3, is_open: true);
                        yield return new WaitForSeconds(speed);
                        highlight_pseudocode(3, is_open: false);
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

                    highlight_pseudocode(8, is_open: true);
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(8, is_open: false);
                    
                    new_node.Set_Scene_Object(Instantiate(node_prefab, view.transform), data);
                    
                    

                    Update_Visual();

                    new_node.Get_GameObject().transform.Set_Child_Active(active: true, 1);
                    yield return new WaitForSeconds(speed);
                    new_node.Get_GameObject().transform.Set_Child_Active(active: false, 1);

                    new_node.Change_Height();
                    StartCoroutine(Rebalance(parents));
                }
            }
            else
            {
                UIHandler.Instance.show_message("Cant have more than 5 levels");
            }
        }


    }

    public IEnumerator delete(long data)
    {
        UIHandler.Instance.close_message();

        BinaryTreeNode current = head;

        Stack<BinaryTreeNode> parents = new Stack<BinaryTreeNode>();
        while (current != null)
        {
            current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return new WaitForSeconds(speed);
            current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            if (data < current.Get_Data())
            {
                parents.Push(current);
                current = current.Get_Left();
            }
            else if (data > current.Get_Data())
            {
                parents.Push(current);
                current = current.Get_Right();
            }
            else if (data == current.Get_Data())
            {
                break;
            }
        }

        if (current == null)
        {
            UIHandler.Instance.show_message("Key doesn't exist");
        }
        else
        {
            if (current.Has_One_Child())
            {
                BinaryTreeNode child_of_current = null;

                if (current.Get_Right() == null)
                    child_of_current = current.Get_Left();
                else if (current.Get_Left() == null)
                    child_of_current = current.Get_Right();

                if (current.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
                    current.Get_Parent()?.Change_Left_To(new_left: child_of_current);
                else
                    current.Get_Parent()?.Change_Right_To(new_right: child_of_current);

                parents.Push(child_of_current);

                if (current == head)
                {
                    head = child_of_current;
                    print("New head is " + head.Get_Data());

                }

                current.Get_GameObject().Destroy_Object();

            }
            else
            {
                if (current.Has_No_Children())
                {
                    if (current.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
                        current.Get_Parent()?.Change_Left_To(new_left: null);
                    else
                        current.Get_Parent()?.Change_Right_To(new_right: null);

                    if (current == head)
                    {
                        head = null;
                        print("New head is " + head?.Get_Data());
                    }

                    current.Get_GameObject().Destroy_Object();
                }
                else
                {
                    BinaryTreeNode right_of_current = current.Get_Right();
                    BinaryTreeNode left_most = right_of_current;
                    BinaryTreeNode left_of_current = current.Get_Left();
                    BinaryTreeNode parent_of_current = current.Get_Parent();

                    while (left_most.Get_Left() != null)
                    {
                        left_most = left_most.Get_Left();
                    }

                    if (left_most.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
                        left_most.Get_Parent().Change_Left_To(new_left: null);
                    else
                        left_most.Get_Parent().Change_Right_To(new_right: null);


                    if (current.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
                    {
                        parent_of_current?.Change_Left_To(new_left: left_most);
                    }
                    else
                    {
                        parent_of_current?.Change_Right_To(new_right: left_most);
                    }

                    if (left_most != right_of_current)
                    {
                        left_most.Change_Right_To(new_right: right_of_current);
                    }

                    left_most.Change_Left_To(new_left: left_of_current);

                    parents.Push(left_most);

                    if (current == head)
                    {
                        head = left_most;
                        print("New head is " + head.Get_Data());
                    }

                    current.Get_GameObject().Destroy_Object();

                }
            }
        }

        Update_Visual();

        StartCoroutine(Rebalance(parents));
    }

    public IEnumerator search(long data)
    {
        UIHandler.Instance.close_message();

        Load_Pseudocode("search");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, is_open: true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, is_open: false);

        if (head == null)
        {
            highlight_pseudocode(1, is_open: true);
            UIHandler.Instance.show_message("Tree is empty.");
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, is_open: false);
        }
        else
        {
            BinaryTreeNode current = head;
            bool exists = false;

            highlight_pseudocode(2, is_open: true);
            head.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, is_open: false);
            head.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            highlight_pseudocode(3, is_open: true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(3, is_open: false);




            while (current != null)
            {
                if (data < current.Get_Data())
                {
                    current = current.Get_Left();

                    highlight_pseudocode(4, is_open: true);
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(4, is_open: false);

                    highlight_pseudocode(5, is_open: true);
                    current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(5, is_open: false);
                    current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

                    highlight_pseudocode(3, is_open: true);
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(3, is_open: false);
                }
                else if (data > current.Get_Data())
                {
                    current = current.Get_Right();

                    highlight_pseudocode(4, is_open: true);
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(4, is_open: false);

                    highlight_pseudocode(6, is_open: true);
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(6, is_open: false);

                    highlight_pseudocode(7, is_open: true);
                    current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(7, is_open: false);
                    current.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

                    highlight_pseudocode(3, is_open: true);
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(3, is_open: false);
                }
                else if (data == current.Get_Data())
                {

                    highlight_pseudocode(4, is_open: true);
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(4, is_open: false);

                    highlight_pseudocode(6, is_open: true);
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(6, is_open: false);

                    highlight_pseudocode(8, is_open: true);
                    UIHandler.Instance.show_message("Key exists");
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(8, is_open: false);
                    exists = true;
                    break;
                }
                
            }
            if (!exists)
            {
                UIHandler.Instance.show_message("Key doesnt exist");
            }
        }
        GameHandler.Instance.is_running = false;
    }
    #endregion

    #region Visual
    private void Update_Visual()
    {

        if (head == null)
            return;

        int position = 0;
        int parent_position = -1;
        BinaryTreeNode current = head;
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

            if (position >= 15)
            {
                current.Get_GameObject().transform.Set_Child_Active(active: false, 2);
                current.Get_GameObject().transform.Set_Child_Active(active: false, 3);
            }
            else
            {
                current.Get_GameObject().transform.Set_Child_Active(active: true, 2);
                current.Get_GameObject().transform.Set_Child_Active(active: true, 3);
            }

            if (current.left == null)
            {
                current.Get_GameObject().transform.Get_Child(2).localScale = Vector3.one;
                current.Get_GameObject().transform.Get_Child(2).eulerAngles = new Vector3(0, 0, 70);
            }

            if (current.right == null)
            {
                current.Get_GameObject().transform.Get_Child(3).localScale = Vector3.one;
                current.Get_GameObject().transform.Get_Child(3).eulerAngles = new Vector3(0, 0, 100);

            }

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

        else if (v.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
        {
            return v.Get_Left();
        }
        else
        {
            return v.Get_Right();
        }
    }

    private IEnumerator Rebalance(Stack<BinaryTreeNode> parents)
    {
        Load_Pseudocode("rebalance");
        yield return new WaitForSeconds(speed);

        while (parents.Count != 0)
        {
            BinaryTreeNode current_parent = parents.Pop();
            BinaryTreeNode v = current_parent;
            BinaryTreeNode w;
            BinaryTreeNode u;

            current_parent.Change_Height();

            current_parent.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return new WaitForSeconds(speed);
            current_parent.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;


            if (!current_parent.Is_Balanced())
            {
                v = current_parent;
                w = Rebalance_Son(v);
                u = Rebalance_Son(w);

                Color_Nodes(v, w, u);

                v = Reconstruct(v, w, u);

                yield return new WaitForSeconds(speed);
                Update_Visual();
                yield return new WaitForSeconds(2 * speed);


                DeColor_Nodes(current_parent, w, u);

                v.Get_Left().Change_Height();
                v.Get_Right().Change_Height();
                v.Change_Height();
            }
            v = v.Get_Parent();
        }
        GameHandler.Instance.is_running = false;
    }

    private void DeColor_Nodes(BinaryTreeNode current_parent, BinaryTreeNode w, BinaryTreeNode u)
    {
        current_parent.Get_GameObject().transform.Set_Child_Active(active: false, 1);
        w.Get_GameObject().transform.Set_Child_Active(active: false, 1);
        w.Get_GameObject().transform.Get_Component_In_Child<MeshRenderer>(1).material = green;
        u.Get_GameObject().transform.Set_Child_Active(active: false, 1);
        u.Get_GameObject().transform.Get_Component_In_Child<MeshRenderer>(1).material = green;
        current_parent.Get_GameObject().transform.Get_Component_In_Child<MeshRenderer>(1).material = green;
    }

    private void Color_Nodes(BinaryTreeNode v, BinaryTreeNode w, BinaryTreeNode u)
    {
        v.Get_GameObject().transform.Set_Child_Active(active: true, 1);
        v.Get_GameObject().transform.Get_Component_In_Child<MeshRenderer>(1).material = red;
        w.Get_GameObject().transform.Set_Child_Active(active: true, 1);
        w.Get_GameObject().transform.Get_Component_In_Child<MeshRenderer>(1).material = green;
        u.Get_GameObject().transform.Set_Child_Active(active: true, 1);
        u.Get_GameObject().transform.Get_Component_In_Child<MeshRenderer>(1).material = blue;
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
                }
                else
                {
                    v.Get_Parent().Change_Right_To(new_right: w);
                }
            }

            v.Change_Left_To(new_left: w.Get_Right());

            w.Change_Right_To(new_right: v);

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
                }
                else
                {
                    v.Get_Parent().Change_Left_To(new_left: w);
                }

            }

            v.Change_Right_To(new_right: w.Get_Left());

            w.Change_Left_To(new_left: v);

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


            w.Change_Left_To(new_left: u.Get_Right());

            if (v != head)
            {
                if (v.Get_Child_Type() == BinaryTreeNode.Child_Type.Right)
                {
                    v.Get_Parent().Change_Right_To(new_right: u);
                }
                else
                {
                    v.Get_Parent().Change_Left_To(new_left: u);
                }
            }

            v.Change_Parent_To(new_parent: u);
            w.Change_Parent_To(new_parent: u);

            u.Change_Left_To(new_left: v);

            u.Change_Right_To(new_right: w);

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

            w.Change_Right_To(new_right: u.Get_Left());

            if (v != head)
            {
                if (v.Get_Child_Type() == BinaryTreeNode.Child_Type.Left)
                {
                    v.Get_Parent().Change_Left_To(new_left: u);
                }
                else
                {

                    v.Get_Parent().Change_Right_To(new_right: u);
                }
            }


            v.Change_Parent_To(new_parent: u);
            w.Change_Parent_To(new_parent: u);
            u.Change_Left_To(new_left: w);
            u.Change_Right_To(new_right: v);
            if (v == head)
            {
                head = u;
                u.Change_Parent_To(new_parent: null);
            }

            return u;

        }

    }
    #endregion


    private int Check_Can_Add(long data)
    {
        BinaryTreeNode current = head;
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
                return pos;
            }
        }

        return pos;
    }
}
