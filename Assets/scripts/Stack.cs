using UnityEngine.UI;
using UnityEngine;

public class Stack : IDataStructure
{
    
    public override void Init()
    {
        view.transform.Destroy_All_Children();

        for (int i = 0; i < init_number; i++)
        {
            create_node();

            if (i < init_number - 1)
            {
                create_arrow();
            }
        }

        view.GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Horizontal;
        view.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        view.GetComponent<GridLayoutGroup>().constraintCount = 1;
        num_nodes = 2;
        max_nodes = 12;
    }

    public void push(long data)
    {
        GameObject node = create_node(data);
        GameObject arrow = create_arrow();
        arrow.transform.SetAsFirstSibling();
        node.transform.SetAsFirstSibling();

        GameHandler.Instance.handle_insertion.Invoke();
    }
}
