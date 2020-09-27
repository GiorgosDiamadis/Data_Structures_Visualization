using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BinaryTree : IDataStructure
{
    public override void Init()
    {
        view.transform.Destroy_All_Children();
        view.GetComponent<GridLayoutGroup>().enabled = false;
        create_node(empty_data:true);
    }
}
