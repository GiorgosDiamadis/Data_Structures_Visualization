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
        normal = GetComponent<Image>().color;
        all = FindObjectsOfType<UIGraphics>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        foreach(UIGraphics g in all)
        {
            if (g != this)
            {
                if(g.GetComponent<Image>().color == highlighted)
                {
                    g.GetComponent<Image>().color = normal;
                }
            }
        }

        if(GetComponent<Image>().color == normal)
        {
            GetComponent<Image>().color = new Color(highlighted.r, highlighted.g, highlighted.b, highlighted.a);
        }
        else
        {
            GetComponent<Image>().color = normal;
        }
    }
}
