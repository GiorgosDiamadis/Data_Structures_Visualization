using System.Collections;
using UnityEngine;
public class SingleLinkedList : IList
{
    public override void create_arrow()
    {
        Instantiate(arrow, view.transform);
    }    
}
