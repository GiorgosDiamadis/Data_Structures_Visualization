using System.Collections;

public interface IQueue
{
    IEnumerator Enqueue(long data);
    void Dequeue();
    void Peek();
}
