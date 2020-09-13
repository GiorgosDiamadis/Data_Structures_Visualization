using System.Collections;
using UnityEngine;
public class List : IDataStructure
{
    [SerializeField] private GameObject arrow = null;
    private static int init_number = 3;

    public override void Init()
    {
        for (int i = 0; i < init_number; i++)
        {
            create_node();

            if (i < init_number - 1)
            {
                create_arrow();
            }
        }
    }
    public GameObject create_arrow()
    {
        GameObject arr = Instantiate(arrow, view.transform);

        return arr;
    }

    protected GameObject create_node(long? data = null)
    {
        new_node = Instantiate(node, view.transform);
        new_node_data = new_node.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        if (!data.HasValue)
            new_node_data.text = (Random.Range(-100, 100)).ToString();
        else
            new_node_data.text = (data.Value).ToString();


        return new_node;

    }

    protected void highlight_pseudocode(int index, bool is_open)
    {
        pseudocode.transform.GetChild(index).GetChild(0).gameObject.SetActive(is_open);
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
            GameObject arrow = create_arrow();

            arrow.transform.SetSiblingIndex(1);
            highlight_pseudocode(0, false);
        }
    }

    public IEnumerator add_position(long data, int position)
    {
        if (!exists(data))
        {
            Load_Pseudocode("add_position");
            yield return new WaitForSeconds(speed);

            if (position == 0)
            {
                highlight_pseudocode(0, true);
                yield return new WaitForSeconds(speed);
                GameObject new_node = create_node(data);

                new_node.transform.SetAsFirstSibling();
                GameObject arrow = create_arrow();

                arrow.transform.SetSiblingIndex(1);
                highlight_pseudocode(0, false);
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

                        spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        k++;
                    }
                }

                spr.sprite = initial_sprite;

                if (!found)
                {
                    highlight_pseudocode(3, true);

                    yield return new WaitForSeconds(speed);


                    print(view.transform.childCount + " " + i + " " + k + " " + position);


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

            spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();
            spr.sprite = initial_sprite;

            if (!found)
            {
                highlight_pseudocode(3, true);

                yield return new WaitForSeconds(speed);
                create_arrow();
                create_node(data);

                highlight_pseudocode(3, false);

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


            Destroy(view.transform.GetChild(position).gameObject);

            if (position == view.transform.childCount - 1)
                Destroy(view.transform.GetChild(position - 1).gameObject);
            else
                Destroy(view.transform.GetChild(position + 1).gameObject);

            highlight_pseudocode(3, false);
        }
        else
        {
            highlight_pseudocode(3, true);

            yield return new WaitForSeconds(speed);
            highlight_pseudocode(3, false);

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
