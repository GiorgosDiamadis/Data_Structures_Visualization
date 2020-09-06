using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class IList : MonoBehaviour, IPointerClickHandler
{
    protected GameObject node = null;
    [SerializeField] protected GameObject arrow = null;
    protected GameObject view = null;
    protected Sprite traverse_sprite = null;
    protected Sprite initial_sprite = null;
    protected float speed = .5f;

    public bool is_init = false;

    private GameObject new_node = null;
    private TMPro.TextMeshProUGUI new_node_data = null;

    protected int init_number = 3;
    protected static GameObject pseudocode = null;
    public abstract void load_pseudocode(string method);

    private void Awake()
    {
        node = Resources.Load("prefabs/Node") as GameObject;
        traverse_sprite = Resources.Load<Sprite>("NeonShapes/PNG/RedCircle");
        initial_sprite = Resources.Load<Sprite>("NeonShapes/PNG/GreenCircle");

        view = GameObject.Find("View");
    }


    public void create_arrow()
    {
        Instantiate(arrow, view.transform);
    }


    public void init_list()
    {
        if (is_init)
            return;

        if (view.transform.childCount > 0)
        {
            for (int i = 0; i < view.transform.childCount; i++)
            {
                Destroy(view.transform.GetChild(i).gameObject);
            }
        }

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

    public IEnumerator add_node(long data)
    {

        load_pseudocode("add");
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
    public IEnumerator delete_node(long data)
    {

        load_pseudocode("delete");
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

        load_pseudocode("search");
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

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (IList list in FindObjectsOfType<IList>())
        {
            if (list != this)
            {
                list.is_init = false;
            }
        }
    }
}
