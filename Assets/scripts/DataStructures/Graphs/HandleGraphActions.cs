using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGraphActions : MonoBehaviour
{
    private static Graphs gr;

    private void Start()
    {
        gr = FindObjectOfType<Graphs>();
    }
    public void Delete()
    {
        gr.Delete();
    }
}
