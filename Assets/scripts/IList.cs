using System.Collections;
using UnityEngine;
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


    private void Awake()
    {
        node = Resources.Load("prefabs/Node") as GameObject;
        arrow = Resources.Load("prefabs/Arrow") as GameObject;
        traverse_sprite = Resources.Load<Sprite>("NeonShapes/PNG/RedCircle");
        initial_sprite = Resources.Load<Sprite>("NeonShapes/PNG/GreenCircle");

        view = GameObject.Find("View");
        pseudocode = GameObject.Find("Pseudocode");
    }

    public IEnumerator search(long data)
    {
        GameObject child;
        for(int i = 0; i < view.transform.childCount; i++)
        {
            child = view.transform.GetChild(i).gameObject;
            if (child.tag.Equals("Node"))
            {
                SpriteRenderer spr = child.transform.GetChild(0).GetComponent<SpriteRenderer>();

                spr.sprite = traverse_sprite;
                TMPro.TextMeshProUGUI child_data = child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                if ( child_data.text == data.ToString())
                {
                    yield return new WaitForSeconds(0.5f);
                    spr.sprite = initial_sprite;
                    break;
                }
                yield return new WaitForSeconds(0.5f);
                spr.sprite = initial_sprite;
            }
        }
    }

}
