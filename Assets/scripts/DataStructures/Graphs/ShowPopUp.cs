using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowPopUp : MonoBehaviour, IPointerClickHandler
{
    public void Start()
    {
        UIHandler.Instance.show_popup += show;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameObject obj = this.transform.Get_Child_Object(1);
            show(obj);
        }
    }

    private void show(GameObject obj)
    {
        Graphs.selected_graph = obj.transform.parent.gameObject;
        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            UIHandler.Instance.scale(obj.GetComponent<RectTransform>(), Vector3.one);
        }
        else
        {
            UIHandler.Instance.scale(obj.GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
        }
    }
}

