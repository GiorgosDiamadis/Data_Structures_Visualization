using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SingleLinkedList : IList
{
    [SerializeField] private bool is_init = false;
    private GameObject new_node = null;
    private TMPro.TextMeshProUGUI new_node_data = null;

    

    public override IEnumerator add_node(long data)
    {
        bool found = false;

        GameObject child,previous;
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

    public override IEnumerator delete_node(long data)
    {
        bool found = false;
        int position = -1;

        GameObject child;
        for (int i = 0; i < view.transform.childCount; i++)
        {
            child = view.transform.GetChild(i).gameObject;
            if (child.tag.Equals("Node"))
            {
                SpriteRenderer spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();

                spr.sprite = traverse_sprite;
                TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                if (child_data.text == data.ToString())
                {
                    yield return new WaitForSeconds(0.5f);

                    spr.sprite = initial_sprite;
                    found = true;
                    position = i;

                    break;
                }
                yield return new WaitForSeconds(0.5f);
                spr.sprite = initial_sprite;
            }
        }

        if (found)
        {
            Destroy(view.transform.GetChild(position).gameObject);

            if(position == view.transform.childCount - 1)
                Destroy(view.transform.GetChild(position - 1).gameObject);
            else
                Destroy(view.transform.GetChild(position + 1).gameObject);

        }
    }

    public override void init_list()
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


    private void create_arrow()
    {
        Instantiate(arrow, view.transform);
    }

    private void create_node(long? data = null)
    {
        new_node = Instantiate(node, view.transform);
        new_node_data = new_node.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        
        if(!data.HasValue)
            new_node_data.text = (Random.Range(-100, 100)).ToString();
        else
            new_node_data.text = (data.Value).ToString();

    }
}
