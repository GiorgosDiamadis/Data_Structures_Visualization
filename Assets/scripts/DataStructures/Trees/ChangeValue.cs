using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeValue : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private TMPro.TMP_InputField inputField = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);
    }
   public void Change_Value()
    {
        inputField.gameObject.SetActive(false);
        long value = long.Parse(inputField.text);
        FindObjectOfType<BinaryTree>().Value_Changed(node_to_be_changed: gameObject, new_value: value);
    }
}
