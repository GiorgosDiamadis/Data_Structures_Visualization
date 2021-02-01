using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeValue : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private TMPro.TMP_InputField inputField = null;
    [SerializeField] private int child;
    [SerializeField] private bool avl;
    public void OnPointerClick(PointerEventData eventData)
    {
        inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);
    }
    public void Change_Value()
    {

        if (inputField.text.Length != 0)
        {
            inputField.gameObject.SetActive(false);
            long value = long.Parse(inputField.text);


            if (value > 1000)
            {
                while (value > 1000)
                    value = value / 10;
            }
            if (!avl)
                FindObjectOfType<BinaryTree>().Value_Changed(node_to_be_changed: gameObject, new_value: value);
            else
                FindObjectOfType<AVLTree>().Value_Changed(node_to_be_changed: gameObject, new_value: value);
        }
        else
        {
            UIHandler.Instance.show_message("Enter a number!");
        }
        
       

    }
}
