using UnityEngine;
using UnityEngine.EventSystems;

public class EnterValue : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMPro.TMP_InputField input_field;
    [SerializeField] private TMPro.TextMeshProUGUI data;
    public void OnPointerClick(PointerEventData eventData)
    {
        input_field.gameObject.SetActive(true);
    }


    public void Apply_Value()
    {
        long value = long.Parse(input_field.text);
        data.text = value.ToString();
        input_field.gameObject.SetActive(false);

        FindObjectOfType<BinaryTree>().Add_To_Array(transform.gameObject);
    }
}
