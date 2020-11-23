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
    public  GraphNode selected_node;
    public  Edge selected_edge;
    private GraphNode from;
    public bool is_digraph = false;

    private static List<GraphNode> adj_list;

    [SerializeField] private GameObject drag_area = null;
    [SerializeField] private GameObject graph_prefab = null;
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
        print("fg");

    }

    public void Add_Weight(Edge edge)
    {
        TMPro.TMP_InputField inf = edge.gameObject.GetComponentInChildren<TMPro.TMP_InputField>();
        int data = int.Parse(inf.text);

        TMPro.TextMeshProUGUI tmpr = edge.gameObject.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0);
        tmpr.text = data.ToString();

        UIHandler.Instance.scale(edge.gameObject.transform.GetChild(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));

        edge.from.Change_Weight(edge.to, data);
        edge.to.Change_Weight(edge.from, data);
    }


    #endregion

    public void Undirected()
    {
        is_digraph = false;
    }


    public void Directed()
    {
        is_digraph = true;
    }


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
        RectTransform actions = selected_edge.gameObject.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        selected_edge.gameObject.GetComponent<Image>().color = Color.white;

        actions.gameObject.SetActive(false);
        actions.localScale = new Vector3(.1f, .1f, .1f);

        actions = obj.gameObject.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_edge = obj;
        selected_edge.gameObject.GetComponent<Image>().color = Color.red;
        selected_edge.gameObject.transform.SetAsLastSibling();
    }

    private void Select_Edge(Edge obj)
    {
        RectTransform actions = obj.gameObject.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_edge = obj;
        selected_edge.gameObject.GetComponent<Image>().color = Color.red;
        selected_edge.gameObject.transform.SetAsLastSibling();

    }

    private void Deselect_Edge()
    {
        RectTransform actions = selected_edge.gameObject.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        UIHandler.Instance.scale(actions, new Vector3(.1f, .1f, .1f));
        selected_edge.gameObject.GetComponent<Image>().color = Color.white;

        selected_edge = null;
    }


    private void Node_Selected(GraphNode obj)
    {
        if (!create_edge)
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
        selected_node.transform.SetAsLastSibling();
    }

    private void Select_Graph(GraphNode obj)
    {
        RectTransform actions = obj.transform.Get_Child_Object(1).GetComponent<RectTransform>();
        actions.gameObject.SetActive(true);

        UIHandler.Instance.scale(actions, Vector3.one);
        selected_node = obj;
        selected_node.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
        selected_node.transform.SetAsLastSibling();

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


        int data = (UnityEngine.Random.Range(-500, 500));

        while (Data_Exists(data))
        {
            data = (UnityEngine.Random.Range(-500, 500));
        }


        new_node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();
        new_node.GetComponent<GraphNode>().data = data;

        adj_list.Add(new_node.GetComponent<GraphNode>());
    }

    public bool Data_Exists(int data)
    {
        foreach (GraphNode g in FindObjectsOfType<GraphNode>())
        {
            if (g.data == data)
            {
                return true;
            }
        }

        return false;
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
            g.Remove_Edges(with: node);
        }
    }

    private void Remove_All_Edges_From_Or_To(GraphNode node)
    {

        foreach (Edge e in node.connections)
        {
            if (e.from == node || e.to == node)
            {
                e.gameObject.Destroy_Object();
            }
        }

        node.connections.RemoveAll(e => e.from == node || e.to == node);
    }

    public void Remove_Edge(Edge edge)
    {
        edge.gameObject.Destroy_Object();

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

        if (Find_Edge(from,to) == null)
        {
            GameObject line = create_arrow();
            line.transform.SetAsLastSibling();

            Quaternion rotation = Quaternion.LookRotation(to.transform.localPosition - from.transform.localPosition, transform.TransformDirection(Vector3.up));

            line.transform.localRotation = new Quaternion(0, 0, rotation.z, rotation.w);
            Quaternion t = new Quaternion(0, 0, -rotation.z, rotation.w);

            line.transform.localPosition = new Vector3((to.transform.localPosition.x + from.transform.localPosition.x) / 2, (to.transform.localPosition.y + from.transform.localPosition.y) / 2, 0);
            float dist = Vector3.Distance(to.transform.localPosition, from.transform.localPosition);
            line.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dist - 100);

            line.transform.Get_Child(1).localRotation = t;


            Edge edge1 = new Edge(from, to, line, 1);
            Edge edge2 = new Edge(to, from, line, 1);

            from.Add_Connection(edge1);
            to.Add_Connection(edge2);
        }
        else
        {
            UIHandler.Instance.show_message("Edge already exists!");
        }
    }


    public IEnumerator DFS(GraphNode from)
    {
        selected_node = null;
        UIHandler.Instance.scale(from.transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
        from.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;


        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        Load_Variables("DFS Traversal:");
        Load_Pseudocode("dfs");

        yield return new WaitForSeconds(speed);

        var visited = new List<GraphNode>();

        var stack = new Stack<GraphNode>();


        stack.Push(from);


        highlight_pseudocode(0, is_open: true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, is_open: false);


        highlight_pseudocode(1, is_open: true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(1, is_open: false);



        while (stack.Count > 0)
        {
            highlight_pseudocode(2, true);
            yield return new WaitForSeconds(speed);
            var vertex = stack.Pop();

            if (visited.Contains(vertex))
                continue;

            highlight_pseudocode(2, false);


            highlight_pseudocode(3, true);
            vertex.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;

            Create_Graph_Node(vertex);
            yield return new WaitForSeconds(2 * speed);
            highlight_pseudocode(3, false);

            vertex.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            visited.Add(vertex);

            int pos = adj_list.IndexOf(vertex);


            highlight_pseudocode(4, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(4, false);
            foreach (var pair in adj_list[pos].connections)
            {

                highlight_pseudocode(5, true);
                yield return new WaitForSeconds(speed);
                highlight_pseudocode(5, false);
                if (!visited.Contains(pair.to))
                {
                    stack.Push(pair.to);
                }


                highlight_pseudocode(4, true);
                yield return new WaitForSeconds(speed);
                highlight_pseudocode(4, false);
            }

            highlight_pseudocode(1, is_open: true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, is_open: false);

        }

        highlight_pseudocode(2, false);
    }

    public IEnumerator BFS(GraphNode from)
    {
        selected_node = null;
        UIHandler.Instance.scale(from.transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
        from.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;


        Load_Variables("BFS Traversal:");
        Load_Pseudocode("bfs");
        if (pseudocode_panel.transform.childCount != 0)
        {
            pseudocode_panel.transform.Get_Child(0, 1).Destroy_All_Children();
        }

        yield return new WaitForSeconds(speed);

        var visited = new List<GraphNode>();

        var queue = new Queue<GraphNode>();

        queue.Enqueue(from);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);




        highlight_pseudocode(1, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(1, false);
        while (queue.Count > 0)
        {
            highlight_pseudocode(2, true);
            yield return new WaitForSeconds(speed);
            var vertex = queue.Dequeue();

            if (visited.Contains(vertex))
                continue;

            highlight_pseudocode(2, false);



            highlight_pseudocode(3, true);
            vertex.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;

            Create_Graph_Node(vertex);
            yield return new WaitForSeconds(2 * speed);
            highlight_pseudocode(3, false);

            vertex.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            visited.Add(vertex);


            int pos = adj_list.IndexOf(vertex);

            highlight_pseudocode(4, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(4, false);
            foreach (var edge in adj_list[pos].connections)
            {
                highlight_pseudocode(5, true);
                yield return new WaitForSeconds(speed);
                highlight_pseudocode(5, false);

                if (!visited.Contains(edge.to))
                {
                    queue.Enqueue(edge.to);
                }

                highlight_pseudocode(4, true);
                yield return new WaitForSeconds(speed);
                highlight_pseudocode(4, false);
            }
                

            highlight_pseudocode(1, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, false);
        }

    }

    private void Create_Graph_Node(GraphNode from)
    {
        int data = from.data;
        GameObject node = Instantiate(graph_prefab);
        node.transform.SetParent(pseudocode_panel.transform.Get_Child(0, 1));
        node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();
        node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).fontSize = 20;
        node.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public IEnumerator Dijkstra(GraphNode source, GraphNode destination)
    {
        selected_node = null;
        Dictionary<GraphNode, int> distance = new Dictionary<GraphNode, int>();
        Dictionary<GraphNode, bool> visited = new Dictionary<GraphNode, bool>();
        Dictionary<GraphNode, GraphNode> parents = new Dictionary<GraphNode, GraphNode>();

        UIHandler.Instance.scale(source.transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));

        Load_Pseudocode("dijkstra");
        yield return new WaitForSeconds(speed);

        foreach (GraphNode g in FindObjectsOfType<GraphNode>())
        {
            foreach (Edge e in g.connections)
            {
                e.gameObject.GetComponent<Image>().color = Color.white;
            }
        }


        foreach (GraphNode g in FindObjectsOfType<GraphNode>())
        {
            g.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(2).text = "Infinity";
            g.transform.Set_Child_Active(true, 2);
            yield return new WaitForSeconds(speed / 2);
        }


        highlight_pseudocode(1, is_open: true);

        yield return new WaitForSeconds(speed);
        foreach (GraphNode g in adj_list)
        {
            distance.Add(g, Int32.MaxValue);
            visited.Add(g, false);
            parents.Add(g, g);
        }

        distance[source] = 0;
        parents[source] = null;
        highlight_pseudocode(1, is_open: false);

        source.gameObject.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(2).text = distance[source].ToString();

        highlight_pseudocode(2, is_open: true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(2, is_open: false);

        while (true)
        {
            highlight_pseudocode(3, is_open: true);
            
            GraphNode node = Find_Cheapest_Unvisited(distance, visited);

            if (node == null || node == destination)
                break;

            node.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;

            yield return new WaitForSeconds(speed);
            highlight_pseudocode(3, is_open: false);

           
            foreach (Edge con in node.connections)
            {
                highlight_pseudocode(4, is_open: true);
                con.gameObject.GetComponent<Image>().color = Color.green;
                yield return new WaitForSeconds(speed);
                highlight_pseudocode(4, is_open: false);

                highlight_pseudocode(5, is_open: true);
                yield return new WaitForSeconds(speed);

                if ((distance[node] + con.weight < distance[con.to]))
                {
                    distance[con.to] = distance[node] + con.weight;

                    parents[con.to]=(node);
                    con.to.gameObject.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(2).text = distance[con.to].ToString();
                }

                highlight_pseudocode(5, is_open: false);

                yield return new WaitForSeconds(speed);

                con.gameObject.GetComponent<Image>().color = Color.white;
            }

            node.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;

            highlight_pseudocode(2, is_open: true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, is_open: false);
        }

        highlight_pseudocode(3, is_open: true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(3, is_open: false);

        GraphNode u1 = destination;

        while(parents[u1]!=null && parents[u1] != u1)
        {

           GraphNode u2 = parents[u1];
            if (u2 != null)
            {
                Edge e = Find_Edge(u1, u2);
                e.gameObject.GetComponent<Image>().color = Color.green;
            }
            u1 = parents[u1];
        }

        yield return new WaitForSeconds(3f);

        foreach (GraphNode g in FindObjectsOfType<GraphNode>())
        {
            foreach (Edge e in g.connections)
            {
                e.gameObject.GetComponent<Image>().color = Color.white;
            }
        }

        foreach (GraphNode g in FindObjectsOfType<GraphNode>())
        {
            g.transform.Set_Child_Active(false, 2);
            g.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(2).text = "Infinity";
        }
    }

    private Edge Find_Edge(GraphNode g1, GraphNode g2)
    {

        foreach (GraphNode g in adj_list)
        {
            foreach (Edge e in g.connections)
            {
                if (e.from == g1 && e.to == g2)
                    return e;
            }
        }
        return null;
    }

    private GraphNode Find_Cheapest_Unvisited(Dictionary<GraphNode, int> distance, Dictionary<GraphNode, bool> visited)
    {
        GraphNode min_node = null;
        int min_dis = Int32.MaxValue;

        foreach (KeyValuePair<GraphNode, int> kv in distance)
        {
            if (kv.Value < min_dis && !visited[kv.Key])
            {
                min_node = kv.Key;
                min_dis = kv.Value;
                visited[min_node] = true;
            }
        }

        return min_node;
    }
    #endregion
}
