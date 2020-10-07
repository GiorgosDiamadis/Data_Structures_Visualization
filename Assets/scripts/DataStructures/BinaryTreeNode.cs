using UnityEngine;

public class BinaryTreeNode
{
    public BinaryTreeNode left;
    public BinaryTreeNode right;
    public BinaryTreeNode parent;
    public GameObject scene_object;

    
    private int height;

    public int position;
    public int parent_position;
    private long data;

    public BinaryTreeNode Get_Parent()
    {
        return parent;
    }

    public void Change_Parent_To(BinaryTreeNode new_parent)
    {
        parent = new_parent;
    }

    public BinaryTreeNode Get_Left()
    {
        return left;
    }

    public BinaryTreeNode Get_Right()
    {
        return right;
    }

    public void Change_Left_To(BinaryTreeNode new_left)
    {
        left = new_left;
    }

    public void Change_Right_To(BinaryTreeNode new_right)
    {
        right = new_right;
    }
    public long Get_Data()
    {
        return data;
    }

    public void Change_Data_To(long new_data)
    {
        data = new_data;
    }

    private int max(int a, int b)
    {
        return (a > b) ? a : b;
    }

    public bool Is_Balanced()
    {
        int balance = Left_Height() - Right_Height();
        return ((-1<=balance) && (balance<=1));
    }

    public void Change_Child_Type(Child_Type type)
    {
        child_type = type;
    }

    public int Right_Height()
    {
        return right == null ? 0 : right.Get_Height();
    }

    public int Left_Height()
    {
        return left == null ? 0 : left.Get_Height();
    }

    public int Get_Height()
    {
        return height;
    }

    public void Change_Height()
    {
        int left_height = left == null ? 0 : left.Get_Height();
        int right_height = right == null ? 0 : right.Get_Height();

        height = 1 + max(left_height, right_height);
    }

    public enum Child_Type
    {
        Left,Right,Root
    }

    public Child_Type child_type;

    public BinaryTreeNode(long data, Child_Type type) : this(data)
    {
        child_type = type;
    }
    public BinaryTreeNode(long data, int position) : this(data)
    {
        this.position = position;
    }
    public BinaryTreeNode(long data)
    {
        this.data = data;
        this.left = null;
        this.right = null;
        this.height = 1;
        this.position = -1;
        this.parent_position = -1;
    }
    public bool Has_A_Child()
    {
        return left != null || right != null;
    }
    public bool Has_Left_Child()
    {
        return left != null;
    }
    public bool Has_Right_Child()
    {
        return right != null;
    }

    
}