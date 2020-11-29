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
    public bool algorithm_running = false;

    [SerializeField] private float speed = 0.1f;

    public Sprite Red_cell { get => red_cell;}
    public Sprite Green_cell { get => green_cell;}
    public  Sprite Traverse_sprite { get => traverse_sprite;}
    public  Sprite Initial_sprite { get => initial_sprite;}
    public float Speed { get => speed;}
    public  GameObject Pseudocode_panel { get => pseudocode_panel; }

    public Action<IDataStructure> On_Data_Structure_Variant_Change { get; internal set; }

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
}
