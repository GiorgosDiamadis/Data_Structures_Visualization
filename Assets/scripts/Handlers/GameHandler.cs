using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;


    private static GameObject pseudocode_panel;

    private static Sprite traverse_sprite = null;
    private static Sprite initial_sprite = null;
    private static Sprite red_cell = null;
    private static Sprite green_cell = null;

    public Action<IDataStructure> On_Data_Structure_Change;
    public Action handle_insertion;
    public Action handle_deletion;
    public bool is_running = false;


    private int MAX_NODES;
    private int num_nodes=3;
    
    [SerializeField] private float speed = 0.1f;

    public Sprite Red_cell { get => red_cell;}
    public Sprite Green_cell { get => green_cell;}
    public  Sprite Traverse_sprite { get => traverse_sprite;}
    public  Sprite Initial_sprite { get => initial_sprite;}
    public float Speed { get => speed;}
    public  GameObject Pseudocode_panel { get => pseudocode_panel; }
    public int max_nodes { get => MAX_NODES; set => MAX_NODES = value; }

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

        pseudocode_panel = GameObject.Find("Pseudocode");

        traverse_sprite = Resources.Load<Sprite>("NeonShapes/PNG/RedCircle");
        initial_sprite = Resources.Load<Sprite>("NeonShapes/PNG/GreenCircle");
        red_cell = Resources.Load<Sprite>("NeonShapes/PNG/RedSquare");
        green_cell = Resources.Load<Sprite>("NeonShapes/PNG/GreenSquare");
    }


    public bool Can_Add()
    {
        return MAX_NODES - num_nodes > 0;
    }
    //Needs to be removed and be shown with pseudocode
    public bool Can_Delete()
    {
        return num_nodes > 0;
    }
}
