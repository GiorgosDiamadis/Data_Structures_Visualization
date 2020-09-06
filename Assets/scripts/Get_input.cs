using System;
using UnityEngine;
public class Get_input : MonoBehaviour
{
    private TMPro.TMP_InputField input_field = null;
    private long data = Int64.MaxValue;

    private void Start()
    {
        input_field = GetComponent<TMPro.TMP_InputField>();
    }

    public void clear()
    {
        input_field.text = "";
    }

    public void add()
    {   
        if(input_field.text.Length != 0)
        {
            data = int.Parse(input_field.text);
        }

        if(data < Int64.MaxValue)
        {
            IList list = transform.GetComponentInParent<IList>();
            StartCoroutine(list.add_front(data));
            data = Int64.MaxValue;
        }

        clear();
    }

    public void search()
    {
        if (input_field.text.Length != 0)
        {
            data = int.Parse(input_field.text);
        }

        if(data < Int64.MaxValue)
        {
            IList list = transform.GetComponentInParent<IList>();
            StartCoroutine(list.search(data));
            data = Int64.MaxValue;
        }
        
        clear();
    }

    public void delete()
    {
        if (input_field.text.Length != 0)
        {
            data = int.Parse(input_field.text);
        }

        if (data < Int64.MaxValue)
        {
            IList list = transform.GetComponentInParent<IList>();
            StartCoroutine(list.delete_node(data));
            data = Int64.MaxValue;
        }
        clear();
    }
}
