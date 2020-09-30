using System;
using UnityEngine;

public static class TransformExtensions
{

    public static void Destroy_Child(this Transform queried,params int[] index)
    {
        Transform child = queried.Get_Child(index);
        child.gameObject.Destroy_Object();
    }

    public static Transform Get_Child(this Transform queried,params int[] index)
    {
        Transform child = queried;
        for (int i = 0; i < index.Length; i++)
        {
            child = child.GetChild(index[i]);
        }
        return child;
    }

    public static T Get_Component_In_Child<T>(this Transform queried,params int[] index) where T : Component
    {
        Transform child = queried.Get_Child(index);
        T requested_component = child.GetComponent<T>();
        return requested_component;
    }

    public static GameObject Get_Child_Object(this Transform queried,params int[] index)
    {
        Transform child = queried.Get_Child(index);

        return child.gameObject;
    }

    public static Transform Get_Child_Transform(this Transform queried, params int[] index)
    {
        Transform child = queried.Get_Child(index);

        return child;
    }

    public static void Set_Child_Active(this Transform queried, bool active,params int[] index)
    {
        Transform child = queried.Get_Child(index);

        child.gameObject.SetActive(active);
    }

    public static void Destroy_All_Children(this Transform queried)
    {
        int child_count = queried.transform.childCount;
        int i = 0;

        while (i < child_count && child_count > 0)
        {
            GameObject obg = queried.transform.GetChild(0).gameObject;
            obg.Destroy_Object();
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
