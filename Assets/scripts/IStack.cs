using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStack
{
    IEnumerator push(long data);
    void peek();
    void pop();
}
