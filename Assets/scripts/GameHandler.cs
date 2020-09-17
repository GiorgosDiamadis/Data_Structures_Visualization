using System;
using UnityEngine;
using DG.Tweening;

public class GameHandler : MonoBehaviour
{
    private IDataStructure current_structure = null;
    private static GameObject view;
    private static GameObject view_panel;
    private static GameObject pseudocode_panel;
    public static GameHandler Instance;
    private static GameObject node = null;
    private static Sprite traverse_sprite = null;
    private static Sprite initial_sprite = null;
    public Action<IDataStructure> On_Data_Structure_Change;
    public Action handle_insertion;
    public Action handle_deletion;

    [SerializeField] private float speed = 0.5f;

    int insertion_counter = 0;
    int deletion_counter = 0;
    private static int MAX_NODES = 14;
    private int num_nodes = 0;
    private void Awake()
    {
        if (Instance == null)
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
        view_panel = GameObject.Find("View_Panel");



        traverse_sprite = Resources.Load<Sprite>("NeonShapes/PNG/RedCircle");
        initial_sprite = Resources.Load<Sprite>("NeonShapes/PNG/GreenCircle");
        node = Resources.Load("prefabs/Node") as GameObject;
        On_Data_Structure_Change += On_Structure_Change;
        handle_insertion += Handle_Insertion;
        handle_deletion += Handle_Deletion;
    }

    public bool Can_Add()
    {
        return MAX_NODES - num_nodes > 0;
    }


    public void Handle_Deletion()
    {
        deletion_counter++;

        num_nodes--;


        if (deletion_counter == 3)
        {
            if (view_panel.transform.localScale.x + 0.2f > 1.0001f)
                return;

            view_panel.transform.localScale = new Vector3(view_panel.transform.localScale.x + 0.2f, view_panel.transform.localScale.y + 0.2f, 0);
            deletion_counter = 0;
        }
    }

    public void Handle_Insertion()
    {
        insertion_counter++;
        
        num_nodes++;

        if (insertion_counter == 3 && view_panel.transform.localScale.x - 0.2f > 0.4f)
        {
            view_panel.transform.localScale = new Vector3(view_panel.transform.localScale.x - 0.2f, view_panel.transform.localScale.y - 0.2f, 0);
            insertion_counter = 0;
        }
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
