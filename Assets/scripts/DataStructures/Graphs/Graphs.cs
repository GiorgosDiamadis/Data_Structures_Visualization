using System;
using System.Collections;
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
        adj_list = new List<GraphNode>();
        drag_area.SetActive(true);
        drop_area.enabled = true;
    }

    public void Add_Weight(Edge edge)
    {
        TMPro.TMP_InputField inf = edge.obj.GetComponentInChildren<TMPro.TMP_InputField>();
        int data = int.Parse(inf.text);

        TMPro.TextMeshProUGUI tmpr = edge.obj.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0);
        tmpr.text = data.ToString();

        UIHandler.Instance.scale(edge.obj.transform.GetChild(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
        
        edge.from.Change_Weight(edge.to,data);
        edge.to.Change_Weight(edge.from,data);
    }


    #endregion

    public override void DeselectStructure()
    {
        base.DeselectStructure();
        view.GetComponent<DropGraphNodeOrArrow>().enabled = false;
        drag_area.SetActive(false);
        drop_area.enabled = false;
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
        RectTransform actions = obj.obj.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        actions.gameObject.SetActive(false);
        actions.localScale = new Vector3(.1f, .1f, .1f);

        actions = obj.obj.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_edge = obj;
    }

    private void Select_Edge(Edge obj)
    {
        RectTransform actions = obj.obj.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_edge = obj;
    }

    private void Deselect_Edge()
    {
        RectTransform actions = selected_edge.obj.transform.Get_Child_Object(1).GetComponent<RectTransform>();
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
            g.Remove_Pairs(with:node);
        }
    }

    private void Remove_All_Edges_From_Or_To(GraphNode node)
    {

        foreach(Edge e in node.connections)
        {
            if(e.from == node || e.to == node)
            {
                e.obj.Destroy_Object();
            }
        }

        node.connections.RemoveAll(e => e.from == node || e.to == node);
    }

    public void Remove_Edge(Edge edge)
    {
        edge.obj.Destroy_Object();

        edge.from.Remove_Edge(edge.to);
        edge.to.Remove_Edge(edge.from);

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
        Quaternion t = new Quaternion(0, 0,-rotation.z, rotation.w);

        line.transform.localPosition = new Vector3((to.transform.localPosition.x + from.transform.localPosition.x) / 2, (to.transform.localPosition.y + from.transform.localPosition.y) / 2, 0);
        float dist = Vector3.Distance(to.transform.localPosition, from.transform.localPosition);
        line.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dist - 100);

        line.transform.Get_Child(1).localRotation = t;


        Edge edge1 = new Edge(from, to, line, 1);
        Edge edge2 = new Edge(to, from, line, 1);

        from.Add_Connection(edge1);
        to.Add_Connection(edge2);
    }

    
    public IEnumerator DFS(GraphNode from)
    {
        selected_node = null;
        UIHandler.Instance.scale(from.transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
        from.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;


        Load_Variables("DFS Traversal:");

        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        yield return new WaitForSeconds(speed);
        
        var visited = new List<GraphNode>();


        var stack = new Stack<GraphNode>();
        stack.Push(from);

        while (stack.Count > 0)
        {
            var vertex = stack.Pop();

            if (visited.Contains(vertex))
                continue;

            vertex.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            Create_Graph_Node(vertex);
            yield return new WaitForSeconds(2*speed);
            vertex.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            visited.Add(vertex);

            int pos = adj_list.IndexOf(vertex);

            foreach (var pair in adj_list[pos].connections)
            {
                if (!visited.Contains(pair.to))
                {
                    stack.Push(pair.to);
                }
            }
        }

    }

    public IEnumerator BFS(GraphNode from)
    {
        selected_node = null;
        UIHandler.Instance.scale(from.transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
        from.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;


        Load_Variables("BFS Traversal:");

        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        yield return new WaitForSeconds(speed);

        var visited = new List<GraphNode>();


        var queue = new Queue<GraphNode>();
        queue.Enqueue(from);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();

            if (visited.Contains(vertex))
                continue;

            vertex.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
            Create_Graph_Node(vertex);
            yield return new WaitForSeconds(2 * speed);
            vertex.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            visited.Add(vertex);

            int pos = adj_list.IndexOf(vertex);

            foreach (var edge in adj_list[pos].connections)
                if (!visited.Contains(edge.to))
                    queue.Enqueue(edge.to);
        }

    }

    private void Create_Graph_Node(GraphNode from)
    {
        int data = from.data;
        GameObject node = Instantiate(graph_prefab);
        node.transform.SetParent(pseudocode_panel.transform.Get_Child(0, 1));
        node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();
        node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).fontSize = 20;
        node.transform.localScale = new Vector3(1f,1f,1f);
    }


    int minDistance(int V, int[] dist, bool[] sptSet)
    {
        int min = Int32.MaxValue;
        int min_index = 0;

        for (int v = 0; v < V; v++)
        {
            if (sptSet[v] == false && dist[v] <= min)
            {
                min = dist[v]; min_index = v;

            }

        }
        return min_index;
    }

    public IEnumerator Dijkstra(GraphNode source, GraphNode destination)
    {
        UIHandler.Instance.scale(source.transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
       
        int[] distance = new int[adj_list.Count];
        Dictionary<int, int> vtxs = new Dictionary<int, int>();
        int[] parents = new int[adj_list.Count];
        bool[] is_set = new bool[adj_list.Count];
        int Vnumber = adj_list.Count;

        for(int i = 0; i < adj_list.Count; i++)
        {
            distance[i] = Int32.MaxValue;
            parents[i] = -1;
            is_set[i] = false;
            vtxs[adj_list[i].data] = i;
        }

        distance[vtxs[source.data]] = 0;

        for(int i = 0; i < Vnumber - 1; i++)
        {
            int u = minDistance(i, distance, is_set);
            is_set[u] = true;

            for (int v = 0; v < Vnumber; v++)
            {
                int dis = distance[v];
                if (!is_set[v] && dis != Int32.MaxValue && distance[u] != Int32.MaxValue
                    && distance[u] + dis < distance[v])
                {
                    distance[v] = distance[u] + dis;
                }
            }
        }


        yield return null;
    }

    private GraphNode Find_Cheapest(Dictionary<GraphNode, int> distance)
    {
        GraphNode min_node = null;
        int min_dis = 0;
        
        foreach(KeyValuePair<GraphNode,int> kv in distance)
        {
            if(min_node == null)
            {
                min_node = kv.Key;
                min_dis = kv.Value;
            }
            else
            {
                if(kv.Value < min_dis)
                {
                    min_node = kv.Key;
                    min_dis = kv.Value;
                }
            }
        }

        return min_node;
    }
    #endregion
}
