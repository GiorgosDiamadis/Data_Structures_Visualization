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
        data = get_data();
        if (data < Int64.MaxValue)
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.add_node(data));
            data = Int64.MaxValue;
        }

        clear();
    }


    public void list_add_position()
    {
        data = get_data();
        position = get_position();

        if (data < Int64.MaxValue)
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.add_position(data, position));
            data = Int64.MaxValue;
        }

        clear();
    }

    private int get_position()
    {
        String[] d = input_field.text.Split(':');
        if (input_field.text.Length != 0)
        {
            position = int.Parse(d[1]);
        }

        return position;
    }

    public void list_add_front()
    {
        data = get_data();

        if (data < Int64.MaxValue)
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.add_front(data));
            data = Int64.MaxValue;
        }

        clear();
    }

    private long get_data()
    {

        String[] d = input_field.text.Split(':');
        if (input_field.text.Length != 0)
        {
            data = int.Parse(d[0]);
        }

        return data;
    }

    public void list_search()
    {
        data = get_data();

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
        data = get_data();

        if (data < Int64.MaxValue)
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.delete_node(data));
            data = Int64.MaxValue;
        }
        clear();
    }
}
