using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

    public class EdgeActions : MonoBehaviour, IPointerClickHandler
    {
        private static Graphs graphs;
        private Edge edge = null;

        private void Start()
        {
            graphs = FindObjectOfType<Graphs>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                foreach (GraphNode g in FindObjectsOfType<GraphNode>())
                {
                    foreach (Edge e in g.connections)
                    {
                        if (e.gameObject == gameObject)
                        {
                            edge = e;
                            break;
                        }
                    }

                }

                Graphs.on_select_edge.Invoke(edge);
            }
        }

        public void Remove()
        {
            graphs.Remove_Edge(edge);
            edge = null;

        }

        public void Add_Weight()
        {
            edge.Add_Weight();
            edge.gameObject.GetComponent<Image>().color = Color.gray;

            edge.gameObject.transform.Get_Component_In_Child<Image>(2, 0).color = Color.gray;
            edge.gameObject.transform.Get_Component_In_Child<Image>(2, 1).color = Color.gray;

            edge.gameObject.transform.Get_Component_In_Child<Image>(3, 0).color = Color.gray;
            edge.gameObject.transform.Get_Component_In_Child<Image>(3, 1).color = Color.gray;
            graphs.selected_edge = null;
        }
    }

