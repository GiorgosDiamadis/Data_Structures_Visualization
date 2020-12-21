using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }


    void Update()
    {
        if(img.color.a > 0.01)
        {
            Color c = img.color;

            c.a -= Time.deltaTime/3;

            img.color = c;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
