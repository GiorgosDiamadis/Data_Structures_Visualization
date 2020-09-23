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
                view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                view.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                view.transform.GetChild(view.transform.childCount - 1).GetChild(2).gameObject.SetActive(true);
            }
        }

        num_nodes = 3;
        max_nodes = 14;

    }
    
    public IEnumerator add_front(long data)
    {

        if (!exists(data))
        {
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
                        view.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                        view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);

                        if (!view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.activeSelf)
                        {
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        view.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                        view.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);

                        view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        view.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

                        if (!view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.activeSelf)
                        {
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(2).gameObject.SetActive(true);
                        }
                    }
                }
            }


            GameHandler.Instance.handle_insertion.Invoke();

        }
    }

    public IEnumerator add_position(long data, int position)
    {

        if (!exists(data))
        {
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
                            view.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                            view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);

                            if (!view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.activeSelf)
                            {
                                view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            view.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                            view.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);

                            view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                            view.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

                            if (!view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.activeSelf)
                            {
                                view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                                view.transform.GetChild(view.transform.childCount - 1).GetChild(2).gameObject.SetActive(true);
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

                            if (!view.transform.GetChild(view.transform.childCount - 3).GetChild(1).gameObject.activeSelf)
                            {
                                view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                            }

                            view.transform.GetChild(view.transform.childCount - 3).GetChild(1).gameObject.SetActive(false);
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                        }
                        else
                        {
                            if (!view.transform.GetChild(view.transform.childCount - 3).GetChild(1).gameObject.activeSelf)
                            {
                                view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                                view.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                            }
                            else
                            {

                                view.transform.GetChild(view.transform.childCount - 3).GetChild(1).gameObject.SetActive(false);
                                view.transform.GetChild(view.transform.childCount - 3).GetChild(2).gameObject.SetActive(false);

                            }


                            view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(2).gameObject.SetActive(true);

                        }
                    }
                    GameHandler.Instance.handle_insertion.Invoke();

                }
            }
        }
    }

    public IEnumerator add_node(long data)
    {
        if (!exists(data))
        {
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

                            if (!view.transform.GetChild(view.transform.childCount - 3).GetChild(1).gameObject.activeSelf)
                            {
                                view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                            }

                            view.transform.GetChild(view.transform.childCount - 3).GetChild(1).gameObject.SetActive(false);
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                        }
                        else
                        {

                            if (!view.transform.GetChild(view.transform.childCount - 3).GetChild(1).gameObject.activeSelf)
                            {
                                view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                                view.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                            }
                            else
                            {

                                view.transform.GetChild(view.transform.childCount - 3).GetChild(1).gameObject.SetActive(false);
                                view.transform.GetChild(view.transform.childCount - 3).GetChild(2).gameObject.SetActive(false);

                            }


                            view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(2).gameObject.SetActive(true);


                        }
                    }

                    GameHandler.Instance.handle_insertion.Invoke();

                }
            }


        }
    }
    public IEnumerator delete_node(long data)
    {

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
            highlight_pseudocode(1, true);

            yield return new WaitForSeconds(speed);

            highlight_pseudocode(1, false);

            highlight_pseudocode(3, true);

            yield return new WaitForSeconds(speed);

            view.transform.GetChild(0).gameObject.Destroy_Object();

            view.transform.GetChild(0).gameObject.Destroy_Object();

            spr.sprite = initial_sprite;

            highlight_pseudocode(3, false);
            if (is_circular)
            {
                if (!is_double)
                {
                    view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                }
                else
                {
                    view.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    view.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
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


                view.transform.GetChild(position).gameObject.Destroy_Object();

                if (position == view.transform.childCount - 1)
                {
                    view.transform.GetChild(position).gameObject.Destroy_Object();
                    if (is_circular)
                    {
                        if (!is_double)
                        {
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                        }
                        else
                        {
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(1).gameObject.SetActive(true);
                            view.transform.GetChild(view.transform.childCount - 1).GetChild(2).gameObject.SetActive(true);
                        }
                    }

                }
                else
                {
                    view.transform.GetChild(position).gameObject.Destroy_Object();
                }


                if (view.transform.childCount == 1)
                {
                    print("skata");
                    view.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    view.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
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


        if (view.transform.childCount == 1)
        {
            print("skata");
            view.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            view.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }

    }

    public IEnumerator search(long data)
    {

        Load_Pseudocode("search");
        yield return new WaitForSeconds(speed);


        GameObject child, previous = null;

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
            yield return new WaitForSeconds(speed);
            spr.sprite = initial_sprite;
            highlight_pseudocode(3, false);
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
                        break;
                    }
                    previous = child;
                }
            }
            highlight_pseudocode(3, true);
            yield return new WaitForSeconds(speed);
            spr.sprite = initial_sprite;

            highlight_pseudocode(3, false);
        }
    }
}
