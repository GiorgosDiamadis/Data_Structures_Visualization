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


    protected int init_number = 3;
    protected GameObject pseudocode = null;
    public abstract void init_list();
    public abstract IEnumerator add_node(long data);
    public abstract IEnumerator delete_node(long data);


    protected float speed = .5f;


    private void Awake()
    {
        node = Resources.Load("prefabs/Node") as GameObject;
        arrow = Resources.Load("prefabs/Arrow") as GameObject;
        traverse_sprite = Resources.Load<Sprite>("NeonShapes/PNG/RedCircle");
        initial_sprite = Resources.Load<Sprite>("NeonShapes/PNG/GreenCircle");

        view = GameObject.Find("View");
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
