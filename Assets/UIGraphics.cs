using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGraphics : MonoBehaviour,IPointerClickHandler
{
    public Color normal;
    public Color highlighted;

    private UIGraphics[] all;

    private void Start()
    {
        normal = transform.Get_Component_In_Child<Image>(0).color;
        all = FindObjectsOfType<UIGraphics>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        foreach(UIGraphics g in all)
        {
            if (g != this)
            {
                if(g.transform.Get_Component_In_Child<Image>(0).color == highlighted)
                {
                    g.transform.Get_Component_In_Child<Image>(0).color = normal;
                }
            }
        }

        if(transform.Get_Component_In_Child<Image>(0).color == normal)
        {
            transform.Get_Component_In_Child<Image>(0).color = new Color(highlighted.r, highlighted.g, highlighted.b, highlighted.a);
        }
        else
        {
            transform.Get_Component_In_Child<Image>(0).color = normal;
        }
    }
}
