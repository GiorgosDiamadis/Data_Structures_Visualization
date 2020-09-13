using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class IDataStructure:MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string pseudocode_dir = "";
    public abstract void init();
    protected static float speed;

    protected static GameObject view = null;
    protected static Sprite traverse_sprite = null;
    protected static Sprite initial_sprite = null;
    protected static GameObject node = null;

    protected static GameObject pseudocode = null;

    protected GameObject new_node = null;
    protected TMPro.TextMeshProUGUI new_node_data = null;

    private void Start()
    {
        view = GameObject.Find("View");
        traverse_sprite = Resources.Load<Sprite>("NeonShapes/PNG/RedCircle");
        initial_sprite = Resources.Load<Sprite>("NeonShapes/PNG/GreenCircle");
        speed = GameHandler.Instance.highlight_speed;
        node = Resources.Load("prefabs/Node") as GameObject;
    }

    protected void load_pseudocode(string method)
    {
        if (pseudocode != null)
            Destroy(pseudocode);

        pseudocode = Resources.Load("prefabs/pseudocode/" + pseudocode_dir + "/pseudocode_" + method) as GameObject;
        pseudocode = Instantiate(pseudocode, FindObjectOfType<Canvas>().transform);
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
}
