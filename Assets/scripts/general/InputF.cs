using System;
using UnityEngine;
using DG.Tweening;

public class InputF : MonoBehaviour
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
        if (GameHandler.Instance.algorithm_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
                AVLTree avl = transform.GetComponentInParent<AVLTree>();
                
                GameHandler.Instance.algorithm_running = true;
            avl.transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

            StartCoroutine(avl.add(data));
                data = Int64.MaxValue;
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }

    public void avl_search()
    {
        if (GameHandler.Instance.algorithm_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
                AVLTree avl = transform.GetComponentInParent<AVLTree>();
                GameHandler.Instance.algorithm_running = true;
            avl.transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

            StartCoroutine(avl.search(data));
                data = Int64.MaxValue;
            
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }

    public void avl_delete()
    {
        if (GameHandler.Instance.algorithm_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
                AVLTree avl = transform.GetComponentInParent<AVLTree>();
            avl.transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

            StartCoroutine(avl.delete(data));
                GameHandler.Instance.algorithm_running = true;
                data = Int64.MaxValue;
            
        }
        else
        {
            UIHandler.Instance.show_message("Please enter a valid number");
        }

        clear();
    }

    public void stack_push()
    {

        if (GameHandler.Instance.algorithm_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (ViewHandler.Instance.Can_Add())
            {
                IStack stack = transform.GetComponentInParent<IStack>();
                GameObject st = GameObject.Find("Stacks");
                st.transform.Get_Component_In_Child<RectTransform>(0,1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);
                st.transform.Get_Component_In_Child<RectTransform>(1, 1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

                StartCoroutine(stack.push(data));
                GameHandler.Instance.algorithm_running = true;
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

    public void queue_enqueue()
    {

        if (GameHandler.Instance.algorithm_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (ViewHandler.Instance.Can_Add())
            {
                IQueue queue= transform.GetComponentInParent<IQueue>();
                GameObject st = GameObject.Find("Queues");
                st.transform.Get_Component_In_Child<RectTransform>(0, 1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);
                st.transform.Get_Component_In_Child<RectTransform>(1, 1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);
                StartCoroutine(queue.Enqueue(data));
                GameHandler.Instance.algorithm_running = true;
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

        if (GameHandler.Instance.algorithm_running)
            return;

        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (ViewHandler.Instance.Can_Add())
            {
                List list = transform.GetComponentInParent<List>();
                GameHandler.Instance.algorithm_running = true;
                list.transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);
                StartCoroutine(list.add_node(data));
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
        if (GameHandler.Instance.algorithm_running)
            return;

        TMPro.TMP_InputField[] input_fields = GetComponentsInChildren<TMPro.TMP_InputField>();

        data = get_data(input_fields[0]);
        position = get_position(input_fields[1]);

        if (data < Int64.MaxValue && ViewHandler.Instance.Can_Add())
        {
            List list = transform.GetComponentInParent<List>();
            GameHandler.Instance.algorithm_running = true;
            list.transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);
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

            if (data > 1000)
            {
                while (data > 1000)
                    data = data / 10;
            }
        }

       

        return data;
    }

    public void list_add_front()
    {
        if (GameHandler.Instance.algorithm_running)
            return;
        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            if (ViewHandler.Instance.Can_Add())
            {
                List list = transform.GetComponentInParent<List>();
                GameHandler.Instance.algorithm_running = true;
                list.transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);
                StartCoroutine(list.add_front(data));
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
        if (GameHandler.Instance.algorithm_running)
            return;
        data = get_data(input_field);

        if (data < Int64.MaxValue)
        {
            List list = transform.GetComponentInParent<List>();
            list.transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);
            StartCoroutine(list.search(data));
            GameHandler.Instance.algorithm_running = true;
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
        if (GameHandler.Instance.algorithm_running)
            return;
        data = get_data(input_field);

       

        if (data < Int64.MaxValue)
        {
            if (ViewHandler.Instance.Can_Delete())
            {
                List list = transform.GetComponentInParent<List>();
                list.transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);
                StartCoroutine(list.delete_node(data));

                GameHandler.Instance.algorithm_running = true;
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
