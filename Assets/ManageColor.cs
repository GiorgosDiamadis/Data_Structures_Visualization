using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageColor : MonoBehaviour
{
    public Color selected;
    void Update()
    {
        if (GetComponent<Button>().IsActive())
        {
            GetComponent<Image>().color= selected;
        }
    }
}
