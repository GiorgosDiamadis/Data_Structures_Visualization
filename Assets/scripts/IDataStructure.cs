using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class IDataStructure:MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string pseudocode_dir = "";
    public abstract void Init();
    protected static float speed;

    protected static GameObject view = null;
    protected static GameObject pseudocode_panel = null;
    protected static Sprite traverse_sprite = null;
    protected static Sprite initial_sprite = null;
    protected int init_number = 3;
    protected int num_nodes;
    protected int max_nodes;
    [SerializeField] protected GameObject node = null;
    [SerializeField] private GameObject arrow = null;

    protected static GameObject pseudocode = null;

    protected GameObject new_node = null;
    protected TMPro.TextMeshProUGUI new_node_data = null;


    public int Get_Max_Nodes()
    {
        return max_nodes;
    }

    private void Start()
    {
        traverse_sprite = GameHandler.Instance.Get_Traverse_Sprite();
        initial_sprite =GameHandler.Instance.Get_Initial_Sprite();
        view = GameHandler.Instance.Get_View();
        speed = GameHandler.Instance.Get_Speed();
        pseudocode_panel = GameHandler.Instance.Get_Pseudocode_Panel();
    }


    protected void highlight_pseudocode(int index, bool is_open)
    {
        pseudocode.transform.GetChild(index).GetChild(0).gameObject.SetActive(is_open);
    }


    protected void Load_Pseudocode(string method)
    {
        if (pseudocode != null)
            Destroy(pseudocode);

        pseudocode = Resources.Load("prefabs/pseudocode/" + pseudocode_dir + "/pseudocode_" + method) as GameObject;
        pseudocode = Instantiate(pseudocode, pseudocode_panel.transform);
        pseudocode.name = "pseudocode_" + method;

        pseudocode.GetComponent<RectTransform>().DOScale(1f, speed);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameHandler.Instance.On_Data_Structure_Change.Invoke(this);
    }

    protected bool exists(long data)
    {
        return view.transform.Does_Data_Exist(data);
    }

    protected GameObject create_arrow()
    {
        GameObject arr = Instantiate(arrow, view.transform);

        return arr;
    }

    public int Get_Counter()
    {
        return num_nodes;
    }

    protected GameObject create_cell(bool not_empty_data=false)
    {
        new_node = Instantiate(node, view.transform);
        new_node_data = new_node.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        if (not_empty_data)
            new_node_data.text = (Random.Range(-100, 100)).ToString();
        else
            new_node_data.text = "";


        return new_node;

    }

    protected GameObject create_node(long? data = null)
    {
        new_node = Instantiate(node, view.transform);
        new_node_data = new_node.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        if (!data.HasValue)
            new_node_data.text = (Random.Range(-100, 100)).ToString();
        else
            new_node_data.text = (data.Value).ToString();


        return new_node;

    }

}
