using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private IDataStructure current_structure = null;
    private static GameObject view;
    private static GameObject pseudocode_panel;
    public static GameHandler Instance;
    private static GameObject node = null;
    private static Sprite traverse_sprite = null;
    private static Sprite initial_sprite = null;
    public Action<IDataStructure> On_Data_Structure_Change;
    [SerializeField] private float speed = 0.5f;

    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
        view = GameObject.Find("View");
        pseudocode_panel = GameObject.Find("Pseudocode");

        print(pseudocode_panel);

        traverse_sprite = Resources.Load<Sprite>("NeonShapes/PNG/RedCircle");
        initial_sprite = Resources.Load<Sprite>("NeonShapes/PNG/GreenCircle");
        node = Resources.Load("prefabs/Node") as GameObject;
        On_Data_Structure_Change += On_Structure_Change;
    }

    public GameObject Get_Pseudocode_Panel()
    {
        return pseudocode_panel;
    }

    public GameObject Get_Node()
    {
        return node;
    }
    public GameObject Get_View()
    {
        return view;
    }

    public Sprite Get_Initial_Sprite()
    {
        return initial_sprite;
    }

    public Sprite Get_Traverse_Sprite()
    {
        return traverse_sprite;
    }

    public float Get_Speed()
    {
        return speed;
    }
    private void On_Structure_Change(IDataStructure structure)
    {
        if (current_structure != structure)
        {
            current_structure = structure;
            view.transform.Destroy_All_Children();
            current_structure.Init();
        }
    }
}
