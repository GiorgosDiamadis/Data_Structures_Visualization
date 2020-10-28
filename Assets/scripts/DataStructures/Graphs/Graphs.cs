using UnityEngine;
using UnityEngine.UI;

public class Graphs : IDataStructure
{
    private Outline drop_area;
    public static int node_count = 0;
    [SerializeField] private GameObject drag_area;
    public static GameObject selected_graph;
    private bool create_edge = false;

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
                    hit.collider.gameObject.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
                }
                else
                {
                    selected_graph.transform.Get_Component_In_Child<Image>(0).sprite = initial_sprite;
                    selected_graph = null;
                }


                create_edge = false;
            }
        }
    }

    public override void Init()
    {
        view.transform.Destroy_All_Children();
        view.GetComponent<DropGraphNodeOrArrow>().enabled = true;
        drop_area = view.GetComponent<Outline>();
        drag_area.SetActive(true);
        drop_area.enabled = true;
    }
    public void Delete()
    {
        selected_graph.Destroy_Object();
        node_count--;

        Update_Matrix();
    }
    public void Add_Edge()
    {
        selected_graph.transform.Get_Component_In_Child<Image>(0).sprite = traverse_sprite;
        print(selected_graph);
        create_edge = true;
    }
    private void Update_Matrix()
    {

    }
}
