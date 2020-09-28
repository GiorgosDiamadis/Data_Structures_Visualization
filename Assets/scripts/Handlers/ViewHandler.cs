using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHandler : MonoBehaviour
{

    public static ViewHandler Instance;
    private int insertion_counter = 0;
    private int deletion_counter = 0;

    private int num_nodes = 3;

    private int counter = 0;
    private static GameObject view;
    private static GameObject view_panel;
    private IDataStructure current_structure = null;

    public GameObject View { get => view; }
    public GameObject View_panel { get => view_panel; }

    private void Start()
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

        GameHandler.Instance.On_Data_Structure_Change += On_Structure_Change;

        GameHandler.Instance.handle_insertion += Handle_Insertion;
        GameHandler.Instance.handle_deletion += Handle_Deletion;
    }


    private void Handle_Deletion()
    {
        deletion_counter++;

        num_nodes--;


        if (deletion_counter == counter && View_panel.transform.localScale.x + 0.2f < 1.1f)
        {
            View_panel.transform.localScale = new Vector3(View_panel.transform.localScale.x + 0.2f, View_panel.transform.localScale.y + 0.2f, 0);
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

        if (insertion_counter == counter && View_panel.transform.localScale.x - 0.2f > 0.4f && View.transform.childCount > 5)
        {
            View_panel.transform.localScale = new Vector3(View_panel.transform.localScale.x - 0.2f, View_panel.transform.localScale.y - 0.2f, 0);
            insertion_counter = 0;
        }
        else if (insertion_counter == counter)
        {
            insertion_counter = 0;
        }
    }

    private void On_Structure_Change(IDataStructure structure)
    {
        if (current_structure != structure)
        {
            View_panel.transform.localScale = new Vector3(1f, 1f, 1f);
            insertion_counter = 0;
            deletion_counter = 0;
            current_structure = structure;
            current_structure.Init();
            counter = current_structure.Max_counter;
            GameHandler.Instance.max_nodes = current_structure.Max_nodes;
        }
    }
}
