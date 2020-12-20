using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewHandler : MonoBehaviour
{

    public static ViewHandler Instance;
    private int insertion_counter = 0;
    private int deletion_counter = 0;

    private int num_nodes = 3;
    private int max_nodes = -1;
    private int counter = 0;
    public static GameObject view;
    private static GameObject view_panel;
    public Action<IDataStructure> on_deselect;
    public IDataStructure current_structure = null;

    private GridLayoutGroup grid;

    public GameObject View { get => view; }
    public GameObject View_panel { get => view_panel; }

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
        view_panel = GameObject.Find("View_Panel");

        grid = view.GetComponent<GridLayoutGroup>();

        GameHandler.Instance.On_Data_Structure_Change += On_Structure_Change;

        GameHandler.Instance.handle_insertion += Handle_Insertion;
        GameHandler.Instance.handle_deletion += Handle_Deletion;
    }

    public void Change_Grid(GridLayoutGroup.Axis axis = GridLayoutGroup.Axis.Horizontal,
        GridLayoutGroup.Constraint constraint = GridLayoutGroup.Constraint.FixedRowCount,
        int constraint_count=1,
        Vector2 spacing=default(Vector2),bool enabled=true,Vector2 size = default(Vector2))
    {
        grid.startAxis = axis;
        grid.constraint = constraint;
        grid.constraintCount = constraint_count;
        grid.spacing = spacing;
        grid.enabled = enabled;
        grid.cellSize = size;
    }

    private void Handle_Deletion()
    {
        deletion_counter++;

        num_nodes--;


        if (deletion_counter == counter && view.transform.localScale.x + 0.2f < 1.1f)
        {
            view.transform.localScale = new Vector3(view.transform.localScale.x + 0.2f, view.transform.localScale.y + 0.2f, 0);
            deletion_counter = 0;
        }
        else if (deletion_counter == counter)
        {
            deletion_counter = 0;
        }
    }

    private void Handle_Insertion()
    {
        insertion_counter++;

        num_nodes++;

        if (insertion_counter == counter && view.transform.localScale.x - 0.2f > 0.4f && view.transform.childCount > 5)
        {
            view.transform.localScale = new Vector3(view.transform.localScale.x - 0.2f, view.transform.localScale.y - 0.2f, 0);
            insertion_counter = 0;
        }
        else if (insertion_counter == counter)
        {
            insertion_counter = 0;
        }
    }


    public bool Can_Add()
    {
        return max_nodes - num_nodes > 0;
    }

    public bool Can_Delete()
    {
        return num_nodes > 0;
    }

    private void On_Structure_Change(IDataStructure structure)
    {
        if (current_structure != structure)
        {
            view.transform.localScale = new Vector3(1f, 1f, 1f);

            current_structure?.DeselectStructure();
            insertion_counter = 0;
            deletion_counter = 0;
            current_structure = structure;
            current_structure.Init();
            UIHandler.Instance.UXinfo("", false);
            current_structure.Destroy_Pseudocode(current_structure);
            counter = current_structure.Max_counter;
            max_nodes = current_structure.Max_nodes;
            GameHandler.Instance.algorithm_running = false;
        }
    }
}
