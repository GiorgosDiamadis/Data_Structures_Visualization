using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueWithArray : IDataStructure
{
    private int next_empty = -1;

    public override void Init()
    {
        view.transform.Destroy_All_Children();
        init_number = 10;

        for (int i = 0; i < init_number; i++)
        {
            if (i < 3)
                create_cell(true);
            else
                create_cell();
        }

        next_empty = 3;
        max_nodes = 10;

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, new Vector2(30f, 30f));
    }

}
