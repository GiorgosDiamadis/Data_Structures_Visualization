using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAddNode : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMPro.TMP_InputField input_field = null;
    private static GameObject clicked;
    private BinaryTree tree;
    private long value;


    public void OnPointerClick(PointerEventData eventData)
    {
        input_field.gameObject.SetActive(!input_field.gameObject.activeSelf);
        clicked = eventData.pointerPress.gameObject;
    }

    public void Add_Node()
    {
        tree = FindObjectOfType<BinaryTree>();
        value = long.Parse(input_field.text);
        input_field.gameObject.SetActive(false);
        tree.Add_Node(clicked,value);
    }
}
