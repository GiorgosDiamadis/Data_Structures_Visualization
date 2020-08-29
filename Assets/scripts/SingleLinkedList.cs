using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SingleLinkedList : IList
{
    [SerializeField] private bool is_init = false;
    private GameObject new_node = null;
    private TMPro.TextMeshProUGUI new_node_data = null;
    private List<string> list;

    public override IEnumerator add_node(long data)
    {
        bool found = false;

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
                    break;
                }
                yield return new WaitForSeconds(0.5f);
                spr.sprite = initial_sprite;
            }
        }

        if (!found)
        {
            create_arrow();
            create_node(data);
            list.Add(data.ToString());
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

        list = new List<string>();

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

        list.Add(new_node_data.text);
    }
}
