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

        

        if (view.transform.childCount > 0)
        {
            UIHandler.Instance.UXinfo("Dequeueing " + view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0, 0).text, true);
            Load_Pseudocode("dequeue");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

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
            Load_Pseudocode("dequeue");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            UIHandler.Instance.show_message("Queue is empty!");
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.algorithm_running = false;
        UIHandler.Instance.UXinfo("", false);
    }

    public IEnumerator Enqueue(long data)
    {
        UIHandler.Instance.close_message();
        UIHandler.Instance.UXinfo("Enqueueing " + data, true);
        GameObject to_add = create_ux_node(data);
        
        Load_Pseudocode("enqueue");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, false);

        if (view.transform.childCount > 1)
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
        
        ViewHandler.Instance.Change_Grid(enabled: true, size: new Vector2(100, 100));
        UIHandler.Instance.UXinfo("", false);
        to_add.Destroy_Object();

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
        

        if (view.transform.childCount == 0)
        {

            Load_Pseudocode("peek");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);
            UIHandler.Instance.show_message("Queue is empty!");
        }
        else
        {
            UIHandler.Instance.UXinfo("Peeking " + view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0, 0).text, true);

            Load_Pseudocode("peek");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

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
