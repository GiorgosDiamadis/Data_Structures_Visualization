﻿using UnityEngine;
using DG.Tweening;

public class DoubleLinkedList : IList
{
    public override void load_pseudocode(string method)
    {
        if (pseudocode != null)
            Destroy(pseudocode);

        pseudocode = Resources.Load("prefabs/pseudocode/dll/pseudocode_" + method) as GameObject;
        pseudocode = Instantiate(pseudocode, FindObjectOfType<Canvas>().transform);
        pseudocode.name = "pseudocode_" + method;

        pseudocode.GetComponent<RectTransform>().DOScale(1f, speed);
    }

}
