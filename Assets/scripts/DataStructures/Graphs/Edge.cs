using UnityEngine;
using UnityEngine.UI;

namespace Graphs
{
    public class Edge
    {
        public GraphNode from;
        public GraphNode to;
        public GameObject gameObject;
        public Graphs graphs;
        public int weight;

        public Edge(GraphNode from, GraphNode to, GameObject obj, int weight)
        {
            this.from = from;
            this.to = to;
            this.gameObject = obj;
            this.weight = weight;
        }

        public void Add_Weight()
        {
            TMPro.TMP_InputField inf = gameObject.GetComponentInChildren<TMPro.TMP_InputField>();
            int data = int.Parse(inf.text);

            TMPro.TextMeshProUGUI tmpr = gameObject.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0);
            tmpr.text = data.ToString();

            UIHandler.Instance.scale(gameObject.transform.GetChild(1).GetComponent<RectTransform>(), new Vector3(.1f, .1f, .1f));
            weight = data;
        }

        public void Remove()
        {

        }
    }


}
