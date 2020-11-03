using System;
using UnityEngine;
using UnityEngine.UI;

public class Graphs : IDataStructure
{
    private Outline drop_area;
    public static Action<GraphNode> on_select_node;
    [SerializeField] private GameObject drag_area = null;
    private static GraphNode selected_node;
    private bool create_edge = false;
    private GraphNode from;


    private void Start()
    {
        on_select_node += Node_Selected;
    }

    public override void Init()
    {
        view.transform.Destroy_All_Children();
        view.GetComponent<DropGraphNodeOrArrow>().enabled = true;
        drop_area = view.GetComponent<Outline>();
        drag_area.SetActive(true);
        drop_area.enabled = true;
    }

    private void Update()
    {
        if (create_edge)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    GraphNode to = hit.collider.gameObject.GetComponent<GraphNode>();

                    Create_Edge(from,to);

                    selected_node.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

                    selected_node = null;

                    from = null;
                }
                else
                {
                    selected_node.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                    selected_node = null;
                }


                create_edge = false;
            }
        }
    }

    private void Create_Edge(GraphNode from, GraphNode to)
    {
        GameObject line = create_arrow();

        Quaternion rotation = Quaternion.LookRotation
 (to.transform.localPosition - from.transform.localPosition, transform.TransformDirection(Vector3.up));

        line.transform.localRotation = new Quaternion(0, 0, rotation.z, rotation.w);

        line.transform.localPosition = new Vector3((to.transform.localPosition.x + from.transform.localPosition.x) / 2, (to.transform.localPosition.y + from.transform.localPosition.y) / 2, 0);
        float dist = Vector3.Distance(to.transform.localPosition, from.transform.localPosition);
        line.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dist - 100);

        from.Add_Connection(to);
        to.Add_Connection(from);

    }



    #region Graph_Node_Selection_Deselection

    private void Node_Selected(GraphNode obj)
    {
        if (selected_node != null && obj != selected_node)
        {
            Select_New_Graph(obj);
        }
        else if (selected_node != null && obj == selected_node)
        {
            Deselect_Graph();
        }
        else
        {
            Select_Graph(obj);
        }
    }

    private void Select_New_Graph(GraphNode obj)
    {
        RectTransform actions = selected_node.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        selected_node.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
        actions.gameObject.SetActive(false);
        actions.localScale = new Vector3(.1f, .1f, .1f);

        actions = obj.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_node = obj;
        selected_node.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
    }

    private void Select_Graph(GraphNode obj)
    {
        RectTransform actions = obj.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_node = obj;
        selected_node.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
    }

    private void Deselect_Graph()
    {
        RectTransform actions = selected_node.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        selected_node.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
        UIHandler.Instance.scale(actions, new Vector3(.1f, .1f, .1f));
        selected_node = null;
    }
    #endregion

    #region Graph_Actions
    public void Delete()
    {
        selected_node.transform.Get_Child_Object(1).Destroy_Object();
        selected_node.gameObject.Destroy_Object();

        selected_node = null;
        Update_Matrix();
    }
    public void Add_Edge(GraphNode from)
    {
        UIHandler.Instance.scale(selected_node.transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
        this.from = from;


        create_edge = true;
    }
    private void Update_Matrix()
    {

    }

    #endregion
}
