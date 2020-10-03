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


    public void avl_add()
    {
        if (GameHandler.Instance.is_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (GameHandler.Instance.Can_Add())
            {
                AVLTree avl = transform.GetComponentInParent<AVLTree>();
                StartCoroutine(avl.add(data));
                GameHandler.Instance.is_running = true;
                data = Int64.MaxValue;
            }
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }

    public void avl_search()
    {
        if (GameHandler.Instance.is_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (GameHandler.Instance.Can_Add())
            {
                AVLTree avl = transform.GetComponentInParent<AVLTree>();
                StartCoroutine(avl.search(data));
                GameHandler.Instance.is_running = true;
                data = Int64.MaxValue;
            }
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }

    public void avl_delete()
    {
        if (GameHandler.Instance.is_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (GameHandler.Instance.Can_Add())
            {
                AVLTree avl = transform.GetComponentInParent<AVLTree>();
                StartCoroutine(avl.delete(data));
                GameHandler.Instance.is_running = true;
                data = Int64.MaxValue;
            }
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }

    public void stack_push()
    {

        if (GameHandler.Instance.is_running)
            return;



        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (GameHandler.Instance.Can_Add())
            {
                IStack stack = transform.GetComponentInParent<IStack>();
                StartCoroutine(stack.push(data));
                GameHandler.Instance.is_running = true;
                data = Int64.MaxValue;
            }
            else
            {
                UIHandler.Instance.show_message("You have reached maximum nodes");

            }
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }

    public void list_add()
    {

        if (GameHandler.Instance.is_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (GameHandler.Instance.Can_Add())
            {
                List list = transform.GetComponentInParent<List>();
                StartCoroutine(list.add_node(data));
                GameHandler.Instance.is_running = true;
                data = Int64.MaxValue;
            }
            else
            {
                UIHandler.Instance.show_message("You have reached maximum nodes");

            }
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }


    public void list_add_position()
    {
        if (GameHandler.Instance.is_running)
            return;

        TMPro.TMP_InputField[] input_fields = GetComponentsInChildren<TMPro.TMP_InputField>();

        data = get_data(input_fields[0]);
        position = get_position(input_fields[1]);

        if (data < Int64.MaxValue && GameHandler.Instance.Can_Add())
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.add_position(data, position));
            GameHandler.Instance.is_running = true;
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
        if (GameHandler.Instance.is_running)
            return;
        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (GameHandler.Instance.Can_Add())
            {
                List list = transform.GetComponentInParent<List>();
                StartCoroutine(list.add_front(data));
                GameHandler.Instance.is_running = true;
                data = Int64.MaxValue;
            }
            else
            {
                UIHandler.Instance.show_message("You have reached maximum nodes");

            }
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }
        clear();
    }

    public void list_search()
    {
        if (GameHandler.Instance.is_running)
            return;
        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            List list = transform.GetComponentInParent<List>();
            StartCoroutine(list.search(data));
            GameHandler.Instance.is_running = true;
            data = Int64.MaxValue;
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }

    public void list_delete()
    {
        if (GameHandler.Instance.is_running)
            return;
        data = get_data(input_field);

       

        if (data < Int64.MaxValue)
        {
            if (GameHandler.Instance.Can_Delete())
            {
                List list = transform.GetComponentInParent<List>();
                StartCoroutine(list.delete_node(data));

                GameHandler.Instance.is_running = true;
                data = Int64.MaxValue;
            }
            else
            {
                UIHandler.Instance.show_message("List is empty");
            }
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }
        clear();
    }
}
