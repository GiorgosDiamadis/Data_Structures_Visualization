using UnityEngine;

public class HandleGraphActions : MonoBehaviour
{
    private static Graphs gr;

    private void Start()
    {
        gr = FindObjectOfType<Graphs>();
    }
    public void Delete()
    {
        gr.Delete();
    }
    public void Add_Edge()
    {
        gr.Add_Edge();
        UIHandler.Instance.show_popup.Invoke(Graphs.selected_graph.transform.Get_Child_Object(1));
    }
}
