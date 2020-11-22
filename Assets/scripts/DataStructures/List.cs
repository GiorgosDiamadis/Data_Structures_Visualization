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

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, new Vector2(10f, 10f));

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
        GameHandler.Instance.is_running = false;
    }

    public IEnumerator add_position(long data, int position)
    {

        if (!exists(data))
        {
            UIHandler.Instance.close_message();
            Load_Pseudocode("add_position");
            yield return new WaitForSeconds(speed);

            if (view.transform.childCount == 0)
            {
                highlight_pseudocode(0, true);
                yield return new WaitForSeconds(speed);
                create_node(data);
                highlight_pseudocode(0, false);
                GameHandler.Instance.handle_insertion.Invoke();
            }
            else if (position == 0)
            {
                highlight_pseudocode(0, true);
                yield return new WaitForSeconds(speed);
                GameObject new_node = create_node(data);

                new_node.transform.SetAsFirstSibling();
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

                SpriteRenderer spr = head.transform.GetChild(0).GetComponent<SpriteRenderer>();
                spr.sprite = traverse_sprite;
                previous = head;

                yield return new WaitForSeconds(speed);
                highlight_pseudocode(0, false);

                int k = 0;
                int i = 1;
                for (; i < view.transform.childCount && k < position - 1; i++)
                {
                    child = view.transform.GetChild(i).gameObject;

                    if (child.tag.Equals("Node"))
                    {

                        highlight_pseudocode(1, true);

                        yield return new WaitForSeconds(speed);

                        highlight_pseudocode(1, false);

                        spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();


                        highlight_pseudocode(2, true);


                        if (previous != null)
                        {
                            previous.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = initial_sprite;
                        }

                        spr.sprite = traverse_sprite;
                        TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                        yield return new WaitForSeconds(speed);

                        highlight_pseudocode(2, false);

                        if (child_data.text == data.ToString())
                        {
                            yield return new WaitForSeconds(speed);
                            spr.sprite = initial_sprite;
                            found = true;
                            break;
                        }

                        yield return new WaitForSeconds(speed);

                        previous = child;

                        spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        k++;
                    }
                }

                spr.sprite = initial_sprite;

                if (!found)
                {
                    highlight_pseudocode(3, true);

                    yield return new WaitForSeconds(speed);

                    if (i == view.transform.childCount)
                    {
                        GameObject arrow = create_arrow();
                        GameObject new_node = create_node(data);

                        new_node.transform.SetSiblingIndex(i + 1);
                        arrow.transform.SetSiblingIndex(i);
                    }
                    else
                    {
                        GameObject arrow = create_arrow();
                        GameObject new_node = create_node(data);

                        if (i % 2 != 0)
                            i++;

                        new_node.transform.SetSiblingIndex(i);
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
            UIHandler.Instance.show_message("Node already exists!");
        }


        GameHandler.Instance.is_running = false;
    }

    public IEnumerator add_node(long data)
    {


        if (!exists(data))
        {
            UIHandler.Instance.close_message();

            Load_Pseudocode("add");
            yield return new WaitForSeconds(speed);

            if (view.transform.childCount == 0)
            {
                highlight_pseudocode(0, true);
                yield return new WaitForSeconds(speed);
                create_node(data);
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

                SpriteRenderer spr = head.transform.GetChild(0).GetComponent<SpriteRenderer>();
                spr.sprite = traverse_sprite;
                previous = head;

                yield return new WaitForSeconds(speed);
                highlight_pseudocode(0, false);

                for (int i = 1; i < view.transform.childCount; i++)
                {
                    child = view.transform.GetChild(i).gameObject;

                    if (child.tag.Equals("Node"))
                    {
                        // While highlighter
                        highlight_pseudocode(1, true);

                        yield return new WaitForSeconds(speed);

                        highlight_pseudocode(1, false);

                        //=========
                        spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();


                        highlight_pseudocode(2, true);


                        if (previous != null)
                        {
                            previous.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = initial_sprite;
                        }

                        spr.sprite = traverse_sprite;
                        TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                        yield return new WaitForSeconds(speed);

                        highlight_pseudocode(2, false);

                        if (child_data.text == data.ToString())
                        {
                            yield return new WaitForSeconds(speed);
                            spr.sprite = initial_sprite;
                            found = true;
                            break;
                        }

                        yield return new WaitForSeconds(speed);

                        previous = child;
                    }
                }

                if (child != null)
                    spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();

                spr.sprite = initial_sprite;

                if (!found)
                {
                    highlight_pseudocode(3, true);

                    yield return new WaitForSeconds(speed);
                    create_arrow();
                    create_node(data);

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
            UIHandler.Instance.show_message("Node already exists");
        }


        GameHandler.Instance.is_running = false;
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

        SpriteRenderer spr = head.transform.GetChild(0).GetComponent<SpriteRenderer>();
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
                    // While highlighter
                    highlight_pseudocode(1, true);

                    yield return new WaitForSeconds(speed);

                    highlight_pseudocode(1, false);

                    //=========
                    spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();


                    highlight_pseudocode(2, true);


                    if (previous != null)
                    {
                        previous.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = initial_sprite;
                    }

                    spr.sprite = traverse_sprite;
                    TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                    yield return new WaitForSeconds(speed);

                    highlight_pseudocode(2, false);

                    if (child_data.text == data.ToString())
                    {
                        yield return new WaitForSeconds(speed);
                        spr.sprite = initial_sprite;
                        found = true;
                        break;
                    }
                    yield return new WaitForSeconds(speed);

                    previous = child;
                }
            }

            spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();
            spr.sprite = initial_sprite;

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
                    view.transform.Set_Child_Active(false, 0, 1);
                    view.transform.Set_Child_Active(false, 0, 2);
                }

                highlight_pseudocode(3, false);

                GameHandler.Instance.handle_deletion.Invoke();
            }
            else
            {
                highlight_pseudocode(3, true);

                yield return new WaitForSeconds(speed);
                highlight_pseudocode(3, false);

            }
        }

        if (!found)
        {
            UIHandler.Instance.show_message("Node doesn't exist");
        }

        GameHandler.Instance.is_running = false;
    }

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

        SpriteRenderer spr = head.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spr.sprite = traverse_sprite;

        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);


        if (head.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text == data.ToString())
        {
            highlight_pseudocode(1, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, false);

            highlight_pseudocode(3, true);
            UIHandler.Instance.show_message("Node found!");
            yield return new WaitForSeconds(speed);
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
                    yield return new WaitForSeconds(speed);
                    highlight_pseudocode(1, false);

                    spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();

                    TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                    highlight_pseudocode(2, true);

                    if (previous != null)
                    {
                        previous.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = initial_sprite;
                    }


                    spr.sprite = traverse_sprite;

                    yield return new WaitForSeconds(speed);
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
            highlight_pseudocode(3, true);
            yield return new WaitForSeconds(speed);
            spr.sprite = initial_sprite;
            if (!found)
            {
                UIHandler.Instance.show_message("Node doesn't exist!");

            }
            highlight_pseudocode(3, false);


        }
        GameHandler.Instance.is_running = false;
    }
}
