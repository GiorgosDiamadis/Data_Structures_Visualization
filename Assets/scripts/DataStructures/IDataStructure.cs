using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.UI;

public abstract class IDataStructure:MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string pseudocode_dir = "";
    public abstract void Init();
    public virtual void DeselectStructure() { }
    protected static float speed;

    protected static GameObject view = null;
    protected static GameObject pseudocode_panel = null;
    
    protected static Sprite traverse_sprite = null;
    protected static Sprite initial_sprite = null;
    protected static Sprite toadd_sprite = null;


    protected static Sprite red_cell = null;
    protected static Sprite green_cell = null;


    protected int init_number = 3;
    protected int max_counter;
    protected int max_nodes;
    [SerializeField] protected GameObject node_prefab = null;
    [SerializeField] protected GameObject arrow = null;

    protected static GameObject pseudocode = null;
    protected static GameObject variable_panel;

    protected GameObject new_node = null;
    protected TMPro.TextMeshProUGUI new_node_data = null;

   

    public int Max_nodes { get => max_nodes;}
    public int Max_counter { get => max_counter;}


    private void Awake()
    {

        traverse_sprite = GameHandler.Instance.Traverse_sprite;
        initial_sprite =GameHandler.Instance.Initial_sprite;
        view = ViewHandler.Instance.View;
        speed = GameHandler.Instance.Speed;
        red_cell= GameHandler.Instance.Red_cell;
        green_cell = GameHandler.Instance.Green_cell;
        toadd_sprite = GameHandler.Instance.Toadd_sprite;
        pseudocode_panel = GameHandler.Instance.Pseudocode_panel;

        //GameHandler.Instance.On_Data_Structure_Change += Destroy_Pseudocode;
    }

    private IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!UnityEngine.Input.GetKeyDown(keyCode))
            yield return null;
        yield return new WaitForFixedUpdate();
    }

    protected IEnumerator Wait()
    {
        if (GameHandler.Instance.step_by_step)
            yield return StartCoroutine(WaitForKeyDown(KeyCode.R));
        else
            yield return new WaitForSeconds(speed);
    }

    public GameObject create_ux_node(long data)
    {
        GameObject to_add = create_node(data, position: new Vector3(0, 200, 0));
        ViewHandler.Instance.Change_Grid(enabled: false);
        to_add.transform.Get_Component_In_Child<Image>(0).sprite = toadd_sprite;

        return to_add;
    }

    public void Destroy_Pseudocode(IDataStructure obj)
    {
        pseudocode_panel.transform.Destroy_All_Children();
    }

    protected void highlight_pseudocode(int index, bool is_open)
    {
        pseudocode.transform.Set_Child_Active(is_open,index,0);
    }

    protected void Load_Variables(string trav)
    {
        if (variable_panel == null || variable_panel.name != trav)
        {
            if (variable_panel != null)
                Destroy(variable_panel);

            variable_panel = Resources.Load("prefabs/pseudocode/Traversal/Traversal") as GameObject;
            variable_panel.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0).text = trav;
            variable_panel = Instantiate(variable_panel, pseudocode_panel.transform);
            variable_panel.name = trav;
            variable_panel.GetComponent<RectTransform>().DOScale(1f, speed);
        }

    }
    protected void Load_Pseudocode(string method)
    {
        if(pseudocode==null || pseudocode.name!= "pseudocode_" + method)
        {
            Destroy(pseudocode);
            pseudocode = Resources.Load("prefabs/pseudocode/" + pseudocode_dir + "/pseudocode_" + method) as GameObject;
            pseudocode = Instantiate(pseudocode, pseudocode_panel.transform);
            pseudocode.name = "pseudocode_" + method;

            pseudocode.GetComponent<RectTransform>().DOScale(1f, speed);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(ViewHandler.Instance.current_structure!=this)
        {
            GameHandler.Instance.On_Data_Structure_Change?.Invoke(this);
            GameHandler.Instance.On_Data_Structure_Variant_Change?.Invoke(this);
        }
        else
        {
            ViewHandler.Instance.current_structure.DeselectStructure();
            ViewHandler.Instance.current_structure = null;
        }
    }

    protected bool exists(long data,bool include_end = true)
    {
        return view.transform.Does_Data_Exist(data,include_end);
    }

    protected GameObject create_arrow()
    {
        GameObject arr = Instantiate(arrow, view.transform);

        return arr;
    }


    protected GameObject create_cell(bool not_empty_data=false)
    {
        new_node = Instantiate(node_prefab, view.transform);
        new_node_data = new_node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0,0);

        if (not_empty_data)
        {
            int x = UnityEngine.Random.Range(-500, 500);
            long data2 = long.Parse(x.ToString());
            new_node_data.text =data2.ToString();

        }
        else
            new_node_data.text = "";


        return new_node;

    }

    protected GameObject create_node(long? data = null,  Vector3? position = null,bool empty_data = false)
    {
        new_node = Instantiate(node_prefab, view.transform);
        new_node_data = new_node.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
       
        if (position.HasValue)
        {
            new_node.transform.localPosition = position.Value;
        }

        if (empty_data)
            new_node_data.text = " ";
        else if (!data.HasValue)
        {
            int x = UnityEngine.Random.Range(-500, 500);
            long data2 = long.Parse(x.ToString());

            new_node_data.text = data2.ToString();

        }
        else if(data.HasValue)
            new_node_data.text = (data.Value).ToString();
        

        return new_node;

    }
}
