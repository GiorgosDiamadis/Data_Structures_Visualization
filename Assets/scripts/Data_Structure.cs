using System.Collections;
using System;
using UnityEngine;

public abstract class Data_Structure : MonoBehaviour
{

    public virtual IEnumerator add(long data)
    {
        yield return null;
    }

    public virtual IEnumerator search(long data)
    {
        yield return null;

    }

    public virtual IEnumerator delete(long data)
    {
        yield return null;

    }

}
