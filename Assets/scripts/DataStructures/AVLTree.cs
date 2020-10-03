using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLTree : BinaryTree
{


    public IEnumerator add(long data)
    {
        int i = 0;

        while (i < tree.Length)
        {
            if (tree[i] < Int64.MaxValue)
            {
                if (data < tree[i])
                    i = i * 2 + 1;
                else
                    i = i * 2 + 2;

            }
            else
            {
                print(i);
                break;
            }
        }
        yield return new WaitForSeconds(speed);

        GameHandler.Instance.is_running = false;
        

    }

    public IEnumerator delete(long data)
    {
        yield return null;
    }

    public IEnumerator search(long data)
    {
        yield return null;
    }
}
