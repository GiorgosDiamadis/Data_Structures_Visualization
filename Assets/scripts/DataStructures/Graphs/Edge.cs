using UnityEngine;
using UnityEngine.UI;

    public class Edge
    {
        public GraphNode from;
        public GraphNode to;
        
        public int weight;

        public GameObject gameObject;
        public Vector3 start;
        public Vector3 end;
        public Graphs graphs;

        public Edge(GraphNode from, GraphNode to, GameObject gameObject,int weight,  Vector3 start, Vector3 end)
        {
            this.from = from;
            this.to = to;
            this.weight = weight;
            this.gameObject = gameObject;
            this.start = start;
            this.end = end;
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



