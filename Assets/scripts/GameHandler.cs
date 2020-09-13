using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private IDataStructure current_structure = null;
    private GameObject view;
    public static GameHandler Instance;
    public Action<IDataStructure> On_Data_Structure_Change;
    [SerializeField] private float speed = 0.5f;
    public float highlight_speed { get { return speed; } }

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
        On_Data_Structure_Change += On_Structure_Change;
    }

    private void On_Structure_Change(IDataStructure structure)
    {
        if (current_structure != structure)
        {
            current_structure = structure;
            view.transform.Destroy_All_Children();
            current_structure.init();
        }
    }
}
