using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleOn : MonoBehaviour
{
    Toggle t;

    private void Start()
    {
        t = GetComponent<Toggle>();
    }
    private void Update()
    {
        if (GameHandler.Instance.algorithm_running)
        {
            t.interactable = false;
        }
        else
        {
            t.interactable = true;
        }
    }
    public void On()
    {
        
        GameHandler.Instance.step_by_step = t.isOn;
        Color c = transform.Get_Component_In_Child<Image>(0).color;

        //if(c == Color.white)
        //{
        //    transform.Get_Component_In_Child<Image>(0).color = Color.green;
        //}
        //else
        //{
        //    transform.Get_Component_In_Child<Image>(0).color = Color.white;

        //}
    }
}
