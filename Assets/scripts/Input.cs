using System;
using UnityEngine;
public class Input : MonoBehaviour
{
    private TMPro.TMP_InputField input_field = null;
    private long data = Int64.MaxValue;
    private int position = -1;

    private void Start()
    {
        input_field = GetComponent<TMPro.TMP_InputField>();
    }
    private void clear()
    {
        input_field.text = "";
    }

    public void list_add()
    {
        data = get_data(input_field);
        if (data < Int64.MaxValue && GameHandler.Instance.Can_Add())
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.add_node(data));
            data = Int64.MaxValue;
        }

        clear();
    }


    public void list_add_position()
    {

        TMPro.TMP_InputField[] input_fields = GetComponentsInChildren<TMPro.TMP_InputField>();

        data = get_data(input_fields[0]);
        position = get_position(input_fields[1]);

        if (data < Int64.MaxValue && GameHandler.Instance.Can_Add())
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.add_position(data, position));
            data = Int64.MaxValue;
        }

        input_fields[0].text = "";
        input_fields[1].text = "";
    }

    private int get_position(TMPro.TMP_InputField inp_f)
    {
        if (inp_f.text.Length != 0)
        {
            position = int.Parse(inp_f.text);
        }

        return position;
    }

    public long get_data(TMPro.TMP_InputField inp_f)
    {

        if (inp_f.text.Length != 0)
        {
            data = int.Parse(inp_f.text);
        }

        return data;
    }

    public void list_add_front()
    {
        data = get_data(input_field);

        if (data < Int64.MaxValue && GameHandler.Instance.Can_Add())
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.add_front(data));
            data = Int64.MaxValue;
        }

        clear();
    }

    

    public void list_search()
    {
        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.search(data));
            data = Int64.MaxValue;
        }

        clear();
    }

    public void list_delete()
    {
        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.delete_node(data));
            data = Int64.MaxValue;
        }
        clear();
    }
}
