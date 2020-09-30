using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAddNode : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        BinaryTree tr = FindObjectOfType<BinaryTree>();
        tr.AddNode(eventData.pointerPress.gameObject);
    }
}
