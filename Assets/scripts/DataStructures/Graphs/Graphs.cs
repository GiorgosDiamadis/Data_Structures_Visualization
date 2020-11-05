using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graphs : IDataStructure
{
    private Outline drop_area;
    public static Action<GraphNode> on_select_node;
    public static Action<Edge> on_select_edge;

    public static Action node_dropped;
    private static GraphNode selected_node;
    private static Edge selected_edge;
    private GraphNode from;
    private List<Edge> edges;

    private List<GraphNode> adj_list;

    [SerializeField] private GameObject drag_area = null;
    [SerializeField] private GameObject graph_prefab= null  ;
    private bool create_edge = false;

    #region Initialize
    private void Start()
    {
        on_select_node += Node_Selected;
        node_dropped += Create_Node;
        on_select_edge += Edge_Selected;
    }

  
    public override void Init()
    {
        view.transform.Destroy_All_Children();
        ViewHandler.Instance.Change_Grid(enabled: false);
        view.GetComponent<DropGraphNodeOrArrow>().enabled = true;
        drop_area = view.GetComponent<Outline>();
        edges = new List<Edge>();
        adj_list = new List<GraphNode>();
        drag_area.SetActive(true);
        drop_area.enabled = true;
    }
    #endregion

    public override void DeselectStructure()
    {
        base.DeselectStructure();
        view.GetComponent<DropGraphNodeOrArrow>().enabled = false;
        drag_area.SetActive(false);
        drop_area.enabled = false;
        edges = null;
        adj_list = null;
    }

    #region Graph_Node_Edge_Selection_Deselection

    private void Edge_Selected(Edge obj)
    {
        if (selected_edge != null && obj != selected_edge)
        {
            Select_New_Edge(obj);
        }
        else if (selected_edge != null && obj == selected_edge)
        {
            Deselect_Edge();
        }
        else
        {
            Select_Edge(obj);
        }
    }

    private void Select_New_Edge(Edge obj)
    {
        RectTransform actions = selected_edge.transform.Get_Child_Object(0).GetComponent<RectTransform>();
        actions.gameObject.SetActive(false);
        actions.localScale = new Vector3(.1f, .1f, .1f);

        actions = obj.transform.Get_Child_Object(0).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_edge = obj;
    }

    private void Select_Edge(Edge obj)
    {
        RectTransform actions = obj.transform.Get_Child_Object(0).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_edge = obj;
    }

    private void Deselect_Edge()
    {
        RectTransform actions = selected_edge.transform.Get_Child_Object(0).GetComponent<RectTransform>();
        UIHandler.Instance.scale(actions, new Vector3(.1f, .1f, .1f));
        selected_edge = null;
    }


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

    private void Create_Node()
    {
        GameObject new_node = Instantiate(graph_prefab);

        new_node.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 90);
        new_node.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90);


        new_node.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, UnityEngine.Input.mousePosition.z));
        new_node.transform.SetParent(ViewHandler.view.transform, true);

        new_node.transform.localScale = Vector3.one;
        new_node.transform.localPosition = new_node.transform.localPosition.With(z: 0);

        int data = (UnityEngine.Random.Range(-100, 100));
        new_node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();
        new_node.GetComponent<GraphNode>().data = data;

        adj_list.Add(new_node.GetComponent<GraphNode>());
    }

    public void Delete_Node()
    {
        selected_node.transform.Get_Child_Object(1).Destroy_Object();

        Remove_All_Edges_From_Or_To(selected_node);
        Remove_Node_From_Adj_List(selected_node);

        adj_list.Remove(selected_node);
        selected_node.gameObject.Destroy_Object();

        selected_node = null;
    }

    private void Remove_Node_From_Adj_List(GraphNode node)
    {
        foreach (GraphNode g in adj_list)
        {
            g.connections.Remove(node);
        }
    }

    private void Remove_All_Edges_From_Or_To(GraphNode node)
    {
        for (int i = 0; i < edges.Count; i++)
        {
            if (edges[i].from == node || edges[i].to == node)
            {
                edges[i].obj.Destroy_Object();
            }
        }

        edges.RemoveAll(e => e.from == node || e.to == node);
    }

    public void Remove_Edge(Edge edge)
    {
        edge.obj.Destroy_Object();

        edge.from.connections.Remove(edge.to);
        edge.to.connections.Remove(edge.from);

        edges.Remove(edge);
    }

    public void Add_Edge(GraphNode from)
    {
        UIHandler.Instance.scale(selected_node.transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
        this.from = from;

        create_edge = true;
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

                    Create_Edge(from, to);

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
        line.transform.SetAsFirstSibling();

        Quaternion rotation = Quaternion.LookRotation(to.transform.localPosition - from.transform.localPosition, transform.TransformDirection(Vector3.up));

        line.transform.localRotation = new Quaternion(0, 0, rotation.z, rotation.w);

        line.transform.localPosition = new Vector3((to.transform.localPosition.x + from.transform.localPosition.x) / 2, (to.transform.localPosition.y + from.transform.localPosition.y) / 2, 0);
        float dist = Vector3.Distance(to.transform.localPosition, from.transform.localPosition);
        line.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dist - 100);


        //Must change
        from.Add_Connection(to);
        to.Add_Connection(from);

        Edge new_edge = line.GetComponent<Edge>();
        new_edge.from = from;
        new_edge.to = to;
        new_edge.obj = line;

        edges.Add(new_edge);
    }

    #endregion
}
