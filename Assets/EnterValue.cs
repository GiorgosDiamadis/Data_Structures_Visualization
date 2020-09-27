using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnterValue : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.parent.GetChild(transform.parent.parent.childCount-1).gameObject.SetActive(true);
    }


    public void Apply_Value()
    {
        long value = long.Parse(transform.parent.parent.GetChild(transform.parent.parent.childCount - 1).GetComponentInChildren<TMPro.TMP_InputField>().text);
        transform.parent.parent.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = value.ToString();
        transform.parent.parent.GetChild(transform.parent.parent.childCount - 1).gameObject.SetActive(false);

    }
}
