using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeValue : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private TMPro.TMP_InputField inputField = null;
    [SerializeField] private int child;
    [SerializeField] private bool avl;
    public void OnPointerClick(PointerEventData eventData)
    {

        //transform.GetChild(child).gameObject.SetActive(!transform.GetChild(child).gameObject.activeSelf);

        print(inputField.gameObject.activeSelf);
        inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);
    }
   public void Change_Value()
    {
        inputField.gameObject.SetActive(false);
        long value = long.Parse(inputField.text);
        if (!avl)
            FindObjectOfType<BinaryTree>().Value_Changed(node_to_be_changed: gameObject, new_value: value);
        else
            FindObjectOfType<AVLTree>().Value_Changed(node_to_be_changed: gameObject, new_value: value);

    }
}
