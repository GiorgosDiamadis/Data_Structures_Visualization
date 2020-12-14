using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueWithList : IDataStructure,IQueue
{
    public void Dequeue()
    {
        if (GameHandler.Instance.algorithm_running)
            return;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

        GameHandler.Instance.algorithm_running = true;
        StartCoroutine(dequeue_cor());
    }

    private IEnumerator dequeue_cor()
    {

        UIHandler.Instance.close_message();


        Load_Pseudocode("dequeue");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, false);

        if (view.transform.childCount > 0)
        {
            highlight_pseudocode(1, true);
            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = traverse_sprite;

            yield return StartCoroutine(Wait());

            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = initial_sprite;
            highlight_pseudocode(1, false);
            view.transform.Destroy_Child(0);

            if (view.transform.childCount > 0)
                view.transform.Destroy_Child(0);

            GameHandler.Instance.handle_deletion.Invoke();

        }
        else
        {
            UIHandler.Instance.show_message("Queue is empty!");
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.algorithm_running = false;
    }

    public IEnumerator Enqueue(long data)
    {
        UIHandler.Instance.close_message();

        Load_Pseudocode("enqueue");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, false);

        if (view.transform.childCount > 0)
        {
            highlight_pseudocode(1, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, false);

            GameObject arrow = create_arrow();

            GameObject node = create_node(data);
        }
        else
        {
            highlight_pseudocode(2, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(2, false);

            GameObject node = create_node(data);
            node.transform.SetAsFirstSibling();
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f );

        GameHandler.Instance.handle_insertion.Invoke();
        GameHandler.Instance.algorithm_running = false;
    }

    public override void Init()
    {
        view.transform.Destroy_All_Children();

        for (int i = 0; i < init_number; i++)
        {
            create_node();

            if (i < init_number - 1)
            {
                create_arrow();
            }
        }

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, size: new Vector2(100, 100));

        max_counter = 3;
        max_nodes = 14;
    }

    public void Peek()
    {
        if (GameHandler.Instance.algorithm_running)
            return;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

        GameHandler.Instance.algorithm_running = true;

        StartCoroutine(peek_cor());
    }

    private IEnumerator peek_cor()
    {

        UIHandler.Instance.close_message();

        Load_Pseudocode("peek");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, false);

        if (view.transform.childCount == 0)
        {
            highlight_pseudocode(2, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(2, false);
            UIHandler.Instance.show_message("Queue is empty!");
        }
        else
        {
            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = traverse_sprite;
            highlight_pseudocode(1, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, false);
            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = initial_sprite;
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.algorithm_running = false;
    }
}
