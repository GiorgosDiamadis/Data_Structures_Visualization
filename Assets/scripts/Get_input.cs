using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Get_input : MonoBehaviour
{
    private TMPro.TMP_InputField input_field = null;
    private int data = -1;

    private void Start()
    {
        input_field = GetComponent<TMPro.TMP_InputField>();
    }

    public void add()
    {
        if(input_field.text.Length != 0)
        {
            data = int.Parse(input_field.text);
        }

        IList list = transform.parent.parent.GetComponent<IList>();
        StartCoroutine(list.add_node(data));
        
    }

    public void search()
    {
        if (input_field.text.Length != 0)
        {
            data = int.Parse(input_field.text);
        }

        IList list = transform.parent.parent.GetComponent<IList>();
        StartCoroutine(list.search(data));
    }

}
