using System.Collections;
using UnityEngine;
using DG.Tweening;
public abstract class IList : MonoBehaviour
{
    protected GameObject node = null;
    protected GameObject arrow = null;
    protected GameObject view = null;
    protected Sprite traverse_sprite = null;
    protected Sprite initial_sprite = null;
    protected float speed = .5f;

    [SerializeField] private bool is_init = false;
    private GameObject new_node = null;
    private TMPro.TextMeshProUGUI new_node_data = null;

    protected int init_number = 3;
    protected GameObject pseudocode = null;
    public abstract void create_arrow();


    private void Awake()
    {
        node = Resources.Load("prefabs/Node") as GameObject;
        arrow = Resources.Load("prefabs/Arrow") as GameObject;
        traverse_sprite = Resources.Load<Sprite>("NeonShapes/PNG/RedCircle");
        initial_sprite = Resources.Load<Sprite>("NeonShapes/PNG/GreenCircle");

        view = GameObject.Find("View");
    }

    public void init_list()
    {
        if (is_init)
            return;
        for (int i = 0; i < init_number; i++)
        {
            create_node();

            if (i < init_number - 1)
            {
                create_arrow();
            }
        }
        is_init = true;
    }
    protected void create_node(long? data = null)
    {
        new_node = Instantiate(node, view.transform);
        new_node_data = new_node.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        if (!data.HasValue)
            new_node_data.text = (Random.Range(-100, 100)).ToString();
        else
            new_node_data.text = (data.Value).ToString();

    }

    protected void highlight_pseudocode(int index, bool is_open)
    {
        pseudocode.transform.GetChild(index).GetChild(0).gameObject.SetActive(is_open);
    }

    protected void load_pseudocode(string method)
    {
        pseudocode = Resources.Load("prefabs/pseudocode/sll/pseudocode_" + method) as GameObject;
        pseudocode = Instantiate(pseudocode, FindObjectOfType<Canvas>().transform);
        pseudocode.name = "pseudocode_" + method;

        pseudocode.GetComponent<RectTransform>().DOScale(1f, speed);
    }

    public IEnumerator add_node(long data)
    {

        if (pseudocode != null)
        {
            if (pseudocode.name != "pseudocode_add")
            {
                Destroy(pseudocode);
                load_pseudocode("add");
                yield return new WaitForSeconds(speed);
            }
        }
        else
        {
            load_pseudocode("add");
            yield return new WaitForSeconds(speed);
        }

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
    public IEnumerator delete_node(long data)
    {
        if (pseudocode != null)
        {
            if (pseudocode.name != "pseudocode_delete")
            {
                Destroy(pseudocode);
                load_pseudocode("delete");
                yield return new WaitForSeconds(speed);
            }
        }
        else
        {
            load_pseudocode("delete");
            yield return new WaitForSeconds(speed);
        }



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
        if (pseudocode != null)
        {
            if (pseudocode.name != "pseudocode_search")
            {
                Destroy(pseudocode);
                load_pseudocode("search");
                yield return new WaitForSeconds(speed);
            }
        }
        else
        {
            load_pseudocode("search");
            yield return new WaitForSeconds(speed);
        }



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
