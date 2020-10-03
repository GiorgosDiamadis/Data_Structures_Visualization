using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeValue : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private TMPro.TMP_InputField inputField;
    public void OnPointerClick(PointerEventData eventData)
    {
        print(gameObject);
        inputField.gameObject.SetActive(true);
    }
   public void Change_Value()
    {
        inputField.gameObject.SetActive(false);
        long value = long.Parse(inputField.text);
        FindObjectOfType<BinaryTree>().Value_Changed(node_to_be_changed: gameObject, new_value: value);
    }
}
