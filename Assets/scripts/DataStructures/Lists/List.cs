﻿using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class List : IDataStructure
{
    [SerializeField] private bool is_circular = false;
    [SerializeField] private bool is_double = false;

    public override void Init()
    {

        view.transform.Destroy_All_Children();

        for (int i = 0; i < init_number; i++)
        {
            create_node();

            if (i < init_number - 1)
            {
                create_arrow();
            }
        }

        if (is_circular)
        {
            if (!is_double)
            {
                view.transform.Set_Child_Active(true, 0, 2);
                view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);

            }
            else
            {
                view.transform.Set_Child_Active(true, 0, 1);
                view.transform.Set_Child_Active(true, 0, 2);


                view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                view.transform.Set_Child_Active(true, view.transform.childCount - 1, 2);

            }
        }

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, new Vector2(-5f, 10f), size: new Vector2(100, 100));

        max_counter = 3;
        max_nodes = 14;
    }

    public IEnumerator add_front(long data)
    {

        if (!exists(data))
        {
            UIHandler.Instance.close_message();
            Load_Pseudocode("add_front");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return new WaitForSeconds(speed);
            GameObject new_node = create_node(data);

            new_node.transform.SetAsFirstSibling();

            if (view.transform.childCount != 1)
            {
                GameObject arrow = create_arrow();
                arrow.transform.SetSiblingIndex(1);
            }

            highlight_pseudocode(0, false);

            if (view.transform.childCount > 2)
            {
                if (is_circular)
                {
                    if (!is_double)
                    {
                        view.transform.Set_Child_Active(false, 2, 2);
                        view.transform.Set_Child_Active(true, 0, 2);


                        if (!view.transform.Get_Child_Object(view.transform.childCount - 1, 1).activeSelf)
                        {
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                        }
                    }
                    else
                    {
                        view.transform.Set_Child_Active(false, 2, 2);
                        view.transform.Set_Child_Active(false, 2, 1);

                        view.transform.Set_Child_Active(true, 0, 2);
                        view.transform.Set_Child_Active(true, 0, 1);


                        if (!view.transform.Get_Child_Object(view.transform.childCount - 1, 1).activeSelf)
                        {
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 2);

                        }
                    }
                }
            }


            GameHandler.Instance.handle_insertion.Invoke();

        }
        else
        {
            UIHandler.Instance.show_message("Node already exists!");
        }
        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

    }

    public IEnumerator add_position(long data, int position)
    {

            GameObject to_add = create_node(data, position: new Vector3(0, 200, 0));
        if (!exists(data,include_end:false))
        {
            UIHandler.Instance.close_message();

            ViewHandler.Instance.Change_Grid(enabled: false);
            to_add.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;
            Load_Pseudocode("add_position");
            yield return new WaitForSeconds(speed);

            if (view.transform.childCount == 0)
            {
                highlight_pseudocode(0, true);
                yield return new WaitForSeconds(speed);
                ViewHandler.Instance.Change_Grid(enabled: true, size: new Vector2(100f, 100f));
                to_add.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

                highlight_pseudocode(0, false);
                GameHandler.Instance.handle_insertion.Invoke();
            }
            else if (position == 0)
            {
                highlight_pseudocode(0, true);
                yield return new WaitForSeconds(speed);
                ViewHandler.Instance.Change_Grid(enabled: true, size: new Vector2(100f, 100f));

                to_add.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                to_add.transform.SetAsFirstSibling();

                GameObject arrow = create_arrow();

                arrow.transform.SetSiblingIndex(1);
                highlight_pseudocode(0, false);

                if (view.transform.childCount > 2)
                {
                    if (is_circular)
                    {
                        if (!is_double)
                        {
                            view.transform.Set_Child_Active(false, 2, 2);
                            view.transform.Set_Child_Active(true, 0, 2);


                            if (!view.transform.Get_Child_Object(view.transform.childCount - 1, 1).activeSelf)
                            {
                                view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                            }
                        }
                        else
                        {
                            view.transform.Set_Child_Active(false, 2, 2);
                            view.transform.Set_Child_Active(false, 2, 1);

                            view.transform.Set_Child_Active(true, 0, 2);
                            view.transform.Set_Child_Active(true, 0, 1);



                            if (!view.transform.Get_Child_Object(view.transform.childCount - 1, 1).activeSelf)
                            {
                                view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                                view.transform.Set_Child_Active(true, view.transform.childCount - 1, 2);

                            }
                        }
                    }
                }

                GameHandler.Instance.handle_insertion.Invoke();
            }

            else
            {
                bool found = false;

                GameObject child, previous;
                child = null;

                GameObject head = view.transform.GetChild(0).gameObject;

                highlight_pseudocode(0, true);

                Image spr = head.transform.GetChild(0).GetComponent<Image>();
                spr.sprite = traverse_sprite;
                previous = head;

                yield return new WaitForSeconds(speed);
                highlight_pseudocode(0, false);

                int k = 0;
                int i = 1;
                for (; i < view.transform.childCount - 1 && k < position - 1; i++)
                {
                    child = view.transform.GetChild(i).gameObject;

                    if (child.tag.Equals("Node"))
                    {

                        highlight_pseudocode(2, false);
                        highlight_pseudocode(1, true);

                        yield return new WaitForSeconds(speed);

                        highlight_pseudocode(1, false);

                        spr = child.transform.GetChild(0).GetComponent<Image>();


                        highlight_pseudocode(2, true);


                        if (previous != null)
                        {
                            previous.transform.GetChild(0).GetComponent<Image>().sprite = initial_sprite;
                        }

                        spr.sprite = traverse_sprite;
                        TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                        yield return new WaitForSeconds(speed);


                        if (child_data.text == data.ToString())
                        {
                            yield return new WaitForSeconds(speed);
                            spr.sprite = initial_sprite;
                            found = true;
                            break;
                        }


                        previous = child;

                        spr = child.transform.GetChild(0).GetComponent<Image>();
                        k++;
                    }
                }
                highlight_pseudocode(2, false);

                spr.sprite = initial_sprite;

                if (!found)
                {
                    highlight_pseudocode(3, true);

                    yield return new WaitForSeconds(speed);

                    if (i == view.transform.childCount - 1)
                    {
                        GameObject arrow = create_arrow();
                        ViewHandler.Instance.Change_Grid(enabled: true, size: new Vector2(100f, 100f));

                        to_add.transform.SetSiblingIndex(i + 1);
                        to_add.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                        arrow.transform.SetSiblingIndex(i);
                    }
                    else
                    {
                        GameObject arrow = create_arrow();
                        to_add.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

                        if (i % 2 != 0)
                            i++;
                        ViewHandler.Instance.Change_Grid(enabled: true, size: new Vector2(100f, 100f));

                        to_add.transform.SetSiblingIndex(i);
                        arrow.transform.SetSiblingIndex(i + 1);
                    }

                    highlight_pseudocode(3, false);

                    if (is_circular)
                    {
                        if (!is_double)
                        {

                            if (!view.transform.Get_Child_Object(view.transform.childCount - 3, 1).activeSelf)
                            {
                                view.transform.Set_Child_Active(true, 0, 2);

                            }

                            view.transform.Set_Child_Active(false, view.transform.childCount - 3, 1);
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);

                        }
                        else
                        {
                            if (!view.transform.Get_Child_Object(view.transform.childCount - 3, 1).activeSelf)
                            {
                                view.transform.Set_Child_Active(true, 0, 2);
                                view.transform.Set_Child_Active(true, 0, 1);

                            }
                            else
                            {
                                view.transform.Set_Child_Active(false, view.transform.childCount - 3, 1);
                                view.transform.Set_Child_Active(false, view.transform.childCount - 3, 2);

                            }

                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 2);


                        }
                    }
                    GameHandler.Instance.handle_insertion.Invoke();

                }
            }
        }
        else
        {
            //to_add.Destroy_Object();
            UIHandler.Instance.show_message("Node already exists!");
        }


        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

    }

    public IEnumerator add_node(long data)
    {

        if (!exists(data))
        {
            UIHandler.Instance.close_message();

            GameObject to_add = create_node(data, position: new Vector3(0, 200, 0));
            ViewHandler.Instance.Change_Grid(enabled: false);
            to_add.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;

            Load_Pseudocode("add");
            yield return new WaitForSeconds(speed);

            if (view.transform.childCount == 0)
            {
                highlight_pseudocode(0, true);
                yield return new WaitForSeconds(speed);
                ViewHandler.Instance.Change_Grid(enabled: true);

                highlight_pseudocode(0, false);

                GameHandler.Instance.handle_insertion.Invoke();
            }
            else
            {

                bool found = false;

                GameObject child, previous;
                child = null;

                GameObject head = view.transform.GetChild(0).gameObject;

                highlight_pseudocode(0, true);

                Image spr = head.transform.GetChild(0).GetComponent<Image>();
                spr.sprite = traverse_sprite;
                previous = head;

                yield return new WaitForSeconds(speed);
                highlight_pseudocode(0, false);

                for (int i = 1; i < view.transform.childCount - 1; i++)
                {
                    child = view.transform.GetChild(i).gameObject;

                    if (child.tag.Equals("Node"))
                    {
                        highlight_pseudocode(2, false);

                        // While highlighter
                        highlight_pseudocode(1, true);

                        yield return new WaitForSeconds(speed);

                        highlight_pseudocode(1, false);

                        //=========
                        spr = child.transform.GetChild(0).GetComponent<Image>();


                        highlight_pseudocode(2, true);


                        if (previous != null)
                        {
                            previous.transform.GetChild(0).GetComponent<Image>().sprite = initial_sprite;
                        }

                        spr.sprite = traverse_sprite;
                        TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                        yield return new WaitForSeconds(speed);


                        if (child_data.text == data.ToString())
                        {

                            highlight_pseudocode(2, false);

                            highlight_pseudocode(1, true);


                            yield return new WaitForSeconds(speed);
                            spr.sprite = initial_sprite;
                            found = true;
                            highlight_pseudocode(1, false);
                            break;
                        }

                        found = false;
                        previous = child;
                    }
                }

                if (child != null)
                    spr = child.transform.GetChild(0).GetComponent<Image>();

                spr.sprite = initial_sprite;
                highlight_pseudocode(2, false);

                if (!found)
                {
                    highlight_pseudocode(3, true);

                    yield return new WaitForSeconds(speed);

                    GameObject arr = create_arrow();
                    ViewHandler.Instance.Change_Grid(enabled: true,size:new Vector2(100f,100f));
                    to_add.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                    arr.transform.SetSiblingIndex(view.transform.childCount - 2);

                    highlight_pseudocode(3, false);

                    if (is_circular)
                    {
                        if (!is_double)
                        {

                            if (!view.transform.Get_Child_Object(view.transform.childCount - 3, 1).gameObject.activeSelf)
                            {
                                view.transform.Set_Child_Active(true, 0, 2);
                            }
                            view.transform.Set_Child_Active(false, view.transform.childCount - 3, 1);
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);

                        }
                        else
                        {

                            if (!view.transform.Get_Child_Object(view.transform.childCount - 3, 1).gameObject.activeSelf)
                            {
                                view.transform.Set_Child_Active(true, 0, 2);
                                view.transform.Set_Child_Active(true, 0, 1);

                            }
                            else
                            {
                                view.transform.Set_Child_Active(false, view.transform.childCount - 3, 1);
                                view.transform.Set_Child_Active(false, view.transform.childCount - 3, 2);

                            }

                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 2);
                        }
                    }

                    GameHandler.Instance.handle_insertion.Invoke();

                }
            }
        }
        else
        {
            UIHandler.Instance.show_message("Node already exists!");
        }
        


        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

    }
    public IEnumerator delete_node(long data)
    {
        UIHandler.Instance.close_message();

        Load_Pseudocode("delete");
        yield return new WaitForSeconds(speed);

        bool found = false;
        int position = -1;

        GameObject child, previous;
        child = null;
        GameObject head = view.transform.GetChild(0).gameObject;

        highlight_pseudocode(0, true);

        Image spr = head.transform.GetChild(0).GetComponent<Image>();
        spr.sprite = traverse_sprite;
        previous = head;

        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);



        if (head.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text == data.ToString())
        {

            found = true;
            highlight_pseudocode(1, true);

            yield return new WaitForSeconds(speed);

            highlight_pseudocode(1, false);

            highlight_pseudocode(3, true);

            yield return new WaitForSeconds(speed);

            view.transform.Destroy_Child(0);

            if (view.transform.childCount > 0)
                view.transform.Destroy_Child(0);

            spr.sprite = initial_sprite;

            highlight_pseudocode(3, false);
            if (is_circular)
            {
                if (!is_double)
                {
                    view.transform.Set_Child_Active(true, 0, 2);
                }
                else
                {
                    view.transform.Set_Child_Active(true, 0, 1);
                    view.transform.Set_Child_Active(true, 0, 2);

                }
            }

            GameHandler.Instance.handle_deletion.Invoke();
        }
        else
        {
            for (int i = 1; i < view.transform.childCount; i++, position++)
            {
                child = view.transform.GetChild(i).gameObject;
                if (child.tag.Equals("Node"))
                {
                    highlight_pseudocode(2, false);

                    // While highlighter
                    highlight_pseudocode(1, true);

                    yield return new WaitForSeconds(speed);

                    highlight_pseudocode(1, false);

                    //=========
                    spr = child.transform.GetChild(0).GetComponent<Image>();


                    highlight_pseudocode(2, true);


                    if (previous != null)
                    {
                        previous.transform.GetChild(0).GetComponent<Image>().sprite = initial_sprite;
                    }

                    spr.sprite = traverse_sprite;
                    TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                    yield return new WaitForSeconds(speed);


                    if (child_data.text == data.ToString())
                    {
                        //yield return new WaitForSeconds(speed);
                        //spr.sprite = initial_sprite;
                        found = true;
                        break;
                    }

                    previous = child;
                }
            }
            highlight_pseudocode(2, false);

            spr = child.transform.GetChild(0).GetComponent<Image>();


            if (found)
            {
                position++;
                highlight_pseudocode(3, true);

                yield return new WaitForSeconds(speed);


                view.transform.Destroy_Child(position);

                if (position == view.transform.childCount - 1)
                {
                    view.transform.Destroy_Child(position);
                    if (is_circular)
                    {
                        if (!is_double)
                        {
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                        }
                        else
                        {
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 1);
                            view.transform.Set_Child_Active(true, view.transform.childCount - 1, 2);

                        }
                    }

                }
                else
                {
                    view.transform.Destroy_Child(position);
                }


                if (view.transform.childCount == 1)
                {
                    if (is_circular)
                    {
                        view.transform.Set_Child_Active(false, 0, 1);
                        view.transform.Set_Child_Active(false, 0, 2);
                    }

                }

                highlight_pseudocode(3, false);

                GameHandler.Instance.handle_deletion.Invoke();
            }
            else
            {
                spr.sprite = initial_sprite;
                highlight_pseudocode(3, true);

                yield return new WaitForSeconds(speed);
                highlight_pseudocode(3, false);

            }
        }

        if (!found)
        {
            UIHandler.Instance.show_message("Node doesn't exist");
        }

        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

    }


    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!UnityEngine.Input.GetKeyDown(keyCode))
            yield return null;
        yield return new WaitForFixedUpdate();
    }

    IEnumerator Wait()
    {
        if (step_by_step)
            yield return StartCoroutine(WaitForKeyDown(KeyCode.Space));
        else
            yield return new WaitForSeconds(speed);
    }


    bool step_by_step = true;
    public IEnumerator search(long data)
    {
        UIHandler.Instance.close_message();
        Load_Pseudocode("search");
        yield return new WaitForSeconds(speed);


        GameObject child, previous = null;
        bool found = false;
        GameObject head = view.transform.GetChild(0).gameObject;
        previous = head;


        highlight_pseudocode(0, true);

        Image spr = head.transform.GetChild(0).GetComponent<Image>();
        spr.sprite = traverse_sprite;

        yield return StartCoroutine(Wait());

        highlight_pseudocode(0, false);


        if (head.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text == data.ToString())
        {
            highlight_pseudocode(1, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, false);

            highlight_pseudocode(3, true);
            UIHandler.Instance.show_message("Node found!");
            yield return StartCoroutine(Wait());
            spr.sprite = initial_sprite;
            highlight_pseudocode(3, false);
            found = true;

        }
        else
        {
            for (int i = 1; i < view.transform.childCount; i++)
            {

                child = view.transform.GetChild(i).gameObject;
                if (child.tag.Equals("Node"))
                {
                    highlight_pseudocode(1, true);
                    print("while");
                    yield return StartCoroutine(Wait());
                    highlight_pseudocode(1, false);

                    spr = child.transform.GetChild(0).GetComponent<Image>();

                    TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                    highlight_pseudocode(2, true);

                    if (previous != null)
                    {
                        previous.transform.GetChild(0).GetComponent<Image>().sprite = initial_sprite;
                    }


                    spr.sprite = traverse_sprite;
                    print("i++");

                    yield return StartCoroutine(Wait());
                    highlight_pseudocode(2, false);

                    if (child_data.text == data.ToString())
                    {

                        UIHandler.Instance.show_message("Node found!");
                        found = true;

                        break;
                    }
                    previous = child;
                }
            }

            highlight_pseudocode(1, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, false);

            highlight_pseudocode(3, true);
            yield return StartCoroutine(Wait());
            spr.sprite = initial_sprite;
            if (!found)
            {
                UIHandler.Instance.show_message("Node doesn't exist!");

            }
            highlight_pseudocode(3, false);


        }
        GameHandler.Instance.algorithm_running = false;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);
    }
}
