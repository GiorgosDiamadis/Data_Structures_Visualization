using System;
using UnityEngine;

public static class TransformExtensions
{
    public static void Destroy_All_Children(this Transform queried)
    {
        int child_count = queried.transform.childCount;
        int i = 0;

        while (i < child_count && child_count > 0)
        {
            GameObject obg = queried.transform.GetChild(0).gameObject;
            obg.transform.SetParent(null);
            UnityEngine.Object.Destroy(obg);
            i++;
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
