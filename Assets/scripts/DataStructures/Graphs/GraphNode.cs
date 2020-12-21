using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public class GraphNode : MonoBehaviour, IPointerClickHandler
    {
        private static Graphs graphs;
        public List<Edge> connections = null;
        public int data;
        [SerializeField] private Sprite initial_sprite = null;

        private void OnEnable()
        {
            graphs = FindObjectOfType<Graphs>();
            connections = new List<Edge>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Graphs.on_select_node.Invoke(this);
            }
        }
        public void Add_Edge()
        {
            graphs.Add_Edge(from: this);
        }

        public void Change_Value()
        {
            TMPro.TMP_InputField n = transform.Get_Component_In_Child<TMPro.TMP_InputField>(1, 0);

            int d = int.Parse(n.text);

            if (graphs.Data_Exists(d))
            {
                UIHandler.Instance.show_message("Data Already Exists!");
            }
            else
            {
                data = d;
                transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = data.ToString();
                UIHandler.Instance.scale(transform.Get_Child_Object(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
                transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                graphs.selected_node = null;
            }
        }

        public void Remove_From_Adj_List()
        {
            foreach (GraphNode g in graphs.adj_list)
            {
                g.Remove_Edges(with: this);
            }

            graphs.adj_list.Remove(this);
            graphs.selected_node = null;
            gameObject.Destroy_Object();

        }

        public void Remove_All_Edges_From_Or_To()
        {

            foreach (GraphNode n in graphs.adj_list)
            {
                foreach (Edge e in n.connections)
                {
                    if (e.from == this || e.to == this)
                    {
                        e.gameObject.Destroy_Object();
                    }
                }
            }


            connections.RemoveAll(e => e.from == this || e.to == this);
        }

        public void Add_Edge(Edge edge)
        {
            connections.Add(edge);
        }


        public void Delete()
        {
            transform.Get_Child_Object(1).Destroy_Object();

            Remove_All_Edges_From_Or_To();
            Remove_From_Adj_List();

            graphs.selected_node = null;
        }

        //When deleting an edge
        public void Remove_Edge(GraphNode to)
        {
            Edge p = null;

            foreach (Edge pa in connections)
            {
                if (pa.to == to)
                {
                    p = pa;
                }
            }

            connections.Remove(p);


            foreach (Edge pa in to.connections)
            {
                if (pa.to == this)
                {
                    p = pa;
                }
            }

            connections.Remove(p);
            graphs.selected_edge = null;
        }


        //When deleting a node
        public void Remove_Edges(GraphNode with)
        {
            connections.RemoveAll(p => p.from == with || p.to == with);
        }


        public void dfs()
        {
            if(!GameHandler.Instance.algorithm_running)
                StartCoroutine(graphs.DFS(this));
        }


        public void bfs()
        {
            if(!GameHandler.Instance.algorithm_running)
                StartCoroutine(graphs.BFS(this));
        }

        public void djkstr()
        {
            GraphNode destination = null;

            TMPro.TMP_InputField n = transform.Get_Component_In_Child<TMPro.TMP_InputField>(1, 6);

            int d = int.Parse(n.text);



            foreach (GraphNode g in graphs.adj_list)
            {
                if (g.data == d)
                {
                    destination = g;
                    break;
                }
            }


            if(!GameHandler.Instance.algorithm_running)
                StartCoroutine(graphs.Dijkstra(this, destination));
        }
    }


