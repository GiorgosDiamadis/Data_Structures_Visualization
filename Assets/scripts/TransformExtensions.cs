using System;
using UnityEngine;

public static class TransformExtensions
{
    public static void Destroy_All_Children(this Transform queried)
    {
        if (queried.childCount > 0)
        {
            for (int i = 0; i < queried.childCount; i++)
            {

                UnityEngine.Object.Destroy(queried.GetChild(i).gameObject);
            }
        }
    }

    public static bool Does_Data_Exist(this Transform queried, long data)
    {
        GameObject child;

        for (int i = 0; i < queried.transform.childCount; i++)
        {
            child = queried.transform.GetChild(i).gameObject;

            if (child.tag.Equals("Node"))
            {
                long node_data = long.Parse(child.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text);
                if (data == node_data)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
