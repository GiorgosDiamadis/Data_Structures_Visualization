using UnityEngine;

public class BinaryTreeNode
{
    public BinaryTreeNode left;
    public BinaryTreeNode right;
    public GameObject scene_object;

    
    public int height;

    public int position;
    public int parent;
    public long data;
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
        this.parent = -1;
    }
    public bool has_not_children()
    {
        return left == null && right == null;
    }
    public bool has_left_child()
    {
        return left != null;
    }
    public bool has_right_child()
    {
        return right != null;
    }

    
}