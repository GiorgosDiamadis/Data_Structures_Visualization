using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinaryTree : IDataStructure
{
    protected static int max_children = 31;
    protected static long[] tree;
    public GameObject e;
    protected Vector3[] positions = new Vector3[31];
    protected float[] rotations_left = new float[15];
    protected float[] rotations_right = new float[15];

    protected Vector3[] scales = new Vector3[15];


    public BinaryTreeNode head;

    public override void Init()
    {
        view.transform.Destroy_All_Children();
        ViewHandler.Instance.Change_Grid(enabled: false);
        tree = new long[max_children];

        for (int i = 0; i < max_children; i++)
        {
            tree[i] = Int64.MaxValue;
        }

        GameObject node =  create_node();
        Add_To_Array(node);

        StartCoroutine (Get_Node_Positions_From_Tree_Prefab());
    }

    private IEnumerator Get_Node_Positions_From_Tree_Prefab()
    {

        for (int i = 0; i < e.transform.childCount; i++)
        {
            positions[i] = e.transform.GetChild(i).localPosition;
        }
        for(int i = 0; i < 15; i++)
        {
            rotations_left[i] = e.transform.Get_Child(i, 3).rotation.eulerAngles.z;
            rotations_right[i] = e.transform.Get_Child(i, 4).rotation.eulerAngles.z;

        }
        for (int i = 0; i < 15; i++)
        {
            scales[i] = e.transform.Get_Child(i, 3).localScale;
        }

        yield return null;
    }
    #region Traversals
    public void In_Order_Traversal()
    {
        if (GameHandler.Instance.algorithm_running)
            return;

        head = null;
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Create_Tree_From_Array();
        StartCoroutine(In_Order_Cor());
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

        GameHandler.Instance.algorithm_running = true;
    }
    private IEnumerator In_Order_Cor()
    {
        Load_Variables("In Order Traversal");
        Load_Pseudocode("inorder");

        yield return new WaitForSeconds(speed);

        Stack<BinaryTreeNode> s = new Stack<BinaryTreeNode>();
        BinaryTreeNode curr = head;
        BinaryTreeNode prev = head;
        highlight_pseudocode(0, is_open: true);
        curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, is_open: false);


        highlight_pseudocode(1, is_open: true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(1, is_open: false);

        // traverse the tree  
        while (curr != null || s.Count > 0)
        {
            highlight_pseudocode(2, is_open: true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(2, is_open: false);

            while (curr != null)
            {
                highlight_pseudocode(3, is_open: true);
                yield return StartCoroutine(Wait());
                highlight_pseudocode(3, is_open: false);

                s.Push(curr);

                curr = curr.left;

                if (curr != null)
                {
                    highlight_pseudocode(4, is_open: true);
                    if (prev != null)
                        prev.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                    prev = curr;
                    curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                    yield return StartCoroutine(Wait());
                    highlight_pseudocode(4, is_open: false);

                }
                else
                {
                    highlight_pseudocode(4, is_open: true);
                    yield return StartCoroutine(Wait());
                    highlight_pseudocode(4, is_open: false);
                }

                highlight_pseudocode(2, is_open: true);
                yield return StartCoroutine(Wait());
                highlight_pseudocode(2, is_open: false);

            }


            highlight_pseudocode(5, is_open: true);
            curr = s.Pop();
            
            if(prev!=null)
                prev.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
            prev = curr;

            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return StartCoroutine(Wait());
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            curr.scene_object.transform.Set_Child_Active(true, 1);

            Create_Pseudocode_Nodes(curr);

            yield return StartCoroutine(Wait());
            highlight_pseudocode(5, is_open: false);


            curr = curr.right;

            if (curr != null)
            {

                highlight_pseudocode(6, is_open: true);
                if (prev != null)
                    prev.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                prev = curr;
                curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                yield return StartCoroutine(Wait());
                highlight_pseudocode(6, is_open: false);

            }
            else
            {
                highlight_pseudocode(6, is_open: true);
                yield return StartCoroutine(Wait());
                highlight_pseudocode(6, is_open: false);
            }

            highlight_pseudocode(1, is_open: true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, is_open: false);
        }


        for (int i = 0; i < view.transform.childCount; i++)
        {
            view.transform.Set_Child_Active(false, i, 1);
        }
        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

    }

    public void Post_Order_Traversal()
    {

        if (GameHandler.Instance.algorithm_running)
            return;

        head = null;
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Create_Tree_From_Array();
        StartCoroutine(Post_Order_Cor());
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

        GameHandler.Instance.algorithm_running = true;
    }
    static public Stack s1, s2;
    private IEnumerator Post_Order_Cor()
    {
        Load_Variables("Post Order Traversal");

        Load_Pseudocode("postorder");

        yield return new WaitForSeconds(speed);

        s1 = new Stack();
        s2 = new Stack();

        s1.Push(head);


        highlight_pseudocode(0, is_open: true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, is_open: false);


        highlight_pseudocode(1, is_open: true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(1, is_open: false);

        while (s1.Count > 0)
        {
            BinaryTreeNode temp = (BinaryTreeNode)s1.Pop();

            

            s2.Push(temp);

            temp.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            highlight_pseudocode(2, is_open: true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(2, is_open: false);

            highlight_pseudocode(3, is_open: true);
            if (temp.left != null)
            {
                temp.left.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;
                s1.Push(temp.left);
            }
            yield return StartCoroutine(Wait());
            if  (temp.left != null)
            {
                temp.left.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            }
            highlight_pseudocode(3, is_open: false);

            highlight_pseudocode(4, is_open: true);
            if (temp.right != null)
            {
                temp.right.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;
                s1.Push(temp.right);
            }
            yield return StartCoroutine(Wait());
            if (temp.left != null)
            {
                temp.left.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            }
            highlight_pseudocode(4, is_open: false);

            highlight_pseudocode(1, is_open: true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, is_open: false);
            temp.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
        }



        highlight_pseudocode(5, is_open: true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(5, is_open: false);
        while (s2.Count > 0)
        {
            BinaryTreeNode temp = (BinaryTreeNode)s2.Pop();

            highlight_pseudocode(6, is_open: true);
            
            temp.scene_object.transform.Set_Child_Active(true, 1);
            Create_Pseudocode_Nodes(temp);
            yield return StartCoroutine(Wait());


            highlight_pseudocode(6, is_open: false);


            highlight_pseudocode(5, is_open: true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(5, is_open: false);
        }

        for (int i = 0; i < view.transform.childCount; i++)
        {
            view.transform.Set_Child_Active(false, i, 1);
        }
        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

    }

    public void Pre_Order_Traversal()
    {

        if (GameHandler.Instance.algorithm_running)
            return;

        head = null;
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Create_Tree_From_Array();
        StartCoroutine(Pre_Order_Cor());
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

        GameHandler.Instance.algorithm_running = true;
    }

    private IEnumerator Pre_Order_Cor()
    {
        Load_Variables("Pre Order Traversal");

        Load_Pseudocode("preorder");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, is_open: true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, is_open: false);

        Stack<BinaryTreeNode> nodeStack = new Stack<BinaryTreeNode>();
        BinaryTreeNode curr = null;

        nodeStack.Push(head);


        highlight_pseudocode(1, is_open: true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(1, is_open: false);

        while (nodeStack.Count > 0)
        {

            highlight_pseudocode(2, is_open: true);
            curr = nodeStack.Peek();

            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return StartCoroutine(Wait());
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;


            nodeStack.Pop();

            curr.scene_object.transform.Set_Child_Active(true, 1);

            Create_Pseudocode_Nodes(curr);

            yield return StartCoroutine(Wait());
            highlight_pseudocode(2, is_open: false);


            highlight_pseudocode(3, is_open: true);

            if (curr.Get_Right() != null)
            {
                curr.Get_Right().scene_object.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;
                nodeStack.Push(curr.Get_Right());
            }

            yield return StartCoroutine(Wait());

            if (curr.Get_Right() != null)
                curr.Get_Right().scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            highlight_pseudocode(3, is_open: false);
            
            highlight_pseudocode(4, is_open: true);
            if (curr.Get_Left() != null)
            {
                curr.Get_Left().scene_object.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;
                nodeStack.Push(curr.Get_Left());
            }

            yield return StartCoroutine(Wait());

            if (curr.Get_Left() != null)
                curr.Get_Left().scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
            highlight_pseudocode(4, is_open: false);


            highlight_pseudocode(1, is_open: true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, is_open: false);
        }

        for (int i = 0; i < view.transform.childCount; i++)
        {
            view.transform.Set_Child_Active(false, i, 1);
        }
        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

    }

    public void Level_Order_Traversal()
    {
        if (GameHandler.Instance.algorithm_running)
            return;

        head = null;
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Create_Tree_From_Array();
        StartCoroutine(Level_Order_Cor());
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

        GameHandler.Instance.algorithm_running = true;
    }
    private IEnumerator Level_Order_Cor()
    {
        Load_Variables("Level Order Traversal"); 
        Load_Pseudocode("levelorder");

        yield return new WaitForSeconds(speed);

        Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
        queue.Enqueue(head);
        
        highlight_pseudocode(0, is_open: true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, is_open: false);


        highlight_pseudocode(1, is_open: true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(1, is_open: false);

        while (queue.Count != 0)
        {
            BinaryTreeNode curr = queue.Dequeue();

            highlight_pseudocode(2, is_open: true);
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            yield return StartCoroutine(Wait());
            curr.scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            curr.scene_object.transform.Set_Child_Active(true, 1);
            Create_Pseudocode_Nodes(curr);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(2, is_open: false);


            highlight_pseudocode(3, is_open: true);
            if(curr.Get_Left() != null)
            {
                curr.Get_Left().scene_object.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;
                queue.Enqueue(curr.Get_Left());
            }
            yield return StartCoroutine(Wait());
            if(curr.Get_Left()!=null)
                curr.Get_Left().scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
            highlight_pseudocode(3, is_open: false);

            highlight_pseudocode(4, is_open: true);
            if (curr.right != null)
            {
                curr.Get_Right().scene_object.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;
                queue.Enqueue(curr.Get_Right());
            }
            yield return StartCoroutine(Wait());
            
            if(curr.Get_Right()!=null)
                curr.Get_Right().scene_object.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
            highlight_pseudocode(4, is_open: false);
            
           

            highlight_pseudocode(1, is_open: true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, is_open: false);
        }

        //yield return StartCoroutine(Wait());
        for (int i = 0; i < view.transform.childCount; i++)
        {
            view.transform.Set_Child_Active(false, i, 1);
        }
        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

    }
    #endregion


    private  void Create_Pseudocode_Nodes(BinaryTreeNode curr)
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

    public void Create_Tree_From_Array()
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

            if (2*position+1 < tree.Length && tree[2 * position + 1] < Int64.MaxValue)
            {
                current.left = new BinaryTreeNode(tree[2 * position + 1], 2 * position + 1);
                current.Get_Left().scene_object = Find_In_View(tree[2 * position + 1]);
                queue.Enqueue(current.Get_Left());
            }

            if (2 * position + 2 < tree.Length && tree[2 * position + 2] < Int64.MaxValue)
            {
                current.right = new BinaryTreeNode(tree[2 * position + 2], 2 * position + 2);
                current.right.scene_object = Find_In_View(tree[2 * position + 2]);
                queue.Enqueue(current.right);

            }
        }
    }

    public void Create_Array_From_Tree()
    {
        Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
        Queue<int> position = new Queue<int>();
        BinaryTreeNode current = null;
        
        for (int j = 0; j < max_children; j++)
        {
            tree[j] = Int64.MaxValue;
        }

        int i = 0;
        queue.Enqueue(head);
        position.Enqueue(i);

        while (queue.Count != 0)
        {
            current = queue.Dequeue();
            int location = position.Dequeue();
            tree[location] = current.Get_Data();

            if (current.Get_Left() != null)
            {
                queue.Enqueue(current.Get_Left());
                position.Enqueue(2 * location + 1);
            }

            if (current.right != null)
            {
                queue.Enqueue(current.right);
                position.Enqueue(2 * location + 2);
            }
        }
    }

    public GameObject Find_In_View(long v)
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

    public int Get_Parent_Position(GameObject node)
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

    public void Add_Node(GameObject clicked_box,long value)
    {

        if (view.transform.Does_Data_Exist(value))
        {
            UIHandler.Instance.show_message("You can't have duplicated nodes");
            return;
        }

        GameObject new_node = Instantiate(node_prefab, clicked_box.transform);

        if (clicked_box.name.Contains("LEFT"))
        {
            new_node.transform.localPosition = new Vector3(-11f, -12f, 0);
            new_node.name = "Left";
        }
        else
        {
            new_node.transform.localPosition = new Vector3(11f, -12f, 0);
            new_node.name = "Right";
        }

        GameObject clicked_box_parent = clicked_box.transform.parent.gameObject;

        Text parent_of_new_node = new_node.GetComponentInChildren<Text>();
        parent_of_new_node.text = clicked_box_parent.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text;

        clicked_box.SetActive(false);

        new_node.transform.SetParent(view.transform);

        new_node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = value.ToString();

        int new_node_position =  Add_To_Array(new_node);

        GameObject parent_of_new_node_object=null;

        for(int i = 0; i < view.transform.childCount; i++)
        {
            if(view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(i,0, 0).text == parent_of_new_node.text)
            {
                parent_of_new_node_object = view.transform.Get_Child_Object(i);
                break;
            }
        }

        int parent_position = Get_Array_Position(parent_of_new_node_object);

        if (new_node.name == "Left")
        {
            parent_of_new_node_object.transform.Get_Child(3).localScale = scales[parent_position];
            parent_of_new_node_object.transform.Get_Child(3).eulerAngles = new Vector3(0,0,rotations_left[parent_position]) ;
        }
        else
        {
            parent_of_new_node_object.transform.Get_Child(4).localScale = scales[parent_position];
            parent_of_new_node_object.transform.Get_Child(4).eulerAngles = new Vector3(0, 0, rotations_right[parent_position]);
        }

        new_node.transform.localPosition = positions[new_node_position];

        if (new_node_position >= 15)
        {
            new_node.transform.Set_Child_Active(active: false, 3);
            new_node.transform.Set_Child_Active(active: false, 4);
            new_node.transform.Set_Child_Active(active: false, 5);
            new_node.transform.Set_Child_Active(active: false, 6);

        }

    }

    public int Get_Array_Position(GameObject node)
    {
        long value = long.Parse(node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text);

        for(int i = 0; i < tree.Length; i++)
        {
            if (tree[i] < Int64.MaxValue)
            {
                if(tree[i] == value)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public int Add_To_Array(GameObject node)
    {
        long value = long.Parse(node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text);
        if (view.transform.childCount == 1)
        {
            tree[0] = value;
            return 0;
        }
        else
        {
            int position = Get_Parent_Position(node);

            if (position == -1)
            {
                return -1;
            }
            else
            {
                if (node.name == "Left")
                {
                    tree[2 * position + 1] = value;
                    return 2 * position + 1;
                }
                else if (node.name == "Right")
                {
                    tree[2 * position + 2] = value;
                    return 2 * position + 2;

                }
            }
        }
        return -1;
    }

    public void Value_Changed(GameObject node_to_be_changed, long new_value)
    {
        int pos = Get_Array_Position(node_to_be_changed);
        tree[pos] = new_value;
       
        node_to_be_changed.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = new_value.ToString();
        Create_Tree_From_Array();
    }
}
