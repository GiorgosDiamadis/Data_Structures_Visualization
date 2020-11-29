using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueWithArray : IDataStructure, IQueue
{
    private int next_enqueue = 3;
    public override void Init()
    {
        view.transform.Destroy_All_Children();
        init_number = 10;

        for (int i = 0; i < init_number; i++)
        {
            if (i < 3)
                create_cell(true);
            else
                create_cell();
        }

        next_enqueue = 3;
        max_nodes = 10;

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, new Vector2(30f, 30f));
    }


    public void Dequeue()
    {
        if (GameHandler.Instance.algorithm_running)
            return;
        GameHandler.Instance.algorithm_running = true;
        StartCoroutine(dequeue_cor());
    }

    private IEnumerator dequeue_cor()
    {
        UIHandler.Instance.close_message();
        Load_Pseudocode("dequeue");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        if (view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0,0,0).text == string.Empty)
        {
            highlight_pseudocode(2, true);
            UIHandler.Instance.show_message("Queue is empty!");
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, false);
        }
        else
        {
            view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = red_cell;
            highlight_pseudocode(1, true);

            yield return new WaitForSeconds(speed);

            highlight_pseudocode(1, false);
            view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = green_cell;
            view.transform.GetChild(0).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = string.Empty;
            next_enqueue--;

            for (int i = 1; i < view.transform.childCount; i++)
            {
                view.transform.GetChild(i).SetSiblingIndex(i - 1);
            }
        }
        GameHandler.Instance.algorithm_running = false;

    }

    public IEnumerator Enqueue(long data)
    {
        UIHandler.Instance.close_message();

        Load_Pseudocode("enqueue");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        if (next_enqueue == 10)
        {
            highlight_pseudocode(2, true);
            UIHandler.Instance.show_message("Queue is full!");
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, false);
        }
        else
        {

            view.transform.GetChild(next_enqueue).GetComponentInChildren<SpriteRenderer>().sprite = red_cell;
            highlight_pseudocode(1, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, false);
            view.transform.GetChild(next_enqueue).GetComponentInChildren<SpriteRenderer>().sprite = green_cell;
            view.transform.GetChild(next_enqueue).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.ToString();

            next_enqueue++;
        }
        GameHandler.Instance.algorithm_running = false;
    }


    public void Peek()
    {
        if (GameHandler.Instance.algorithm_running)
            return;

        GameHandler.Instance.algorithm_running = true;

        StartCoroutine(peek_cor());
    }

    private IEnumerator peek_cor()
    {
        UIHandler.Instance.close_message();
        yield return new WaitForSeconds(speed);

        Load_Pseudocode("peek");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        if (view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0, 0).text == string.Empty)
        {
            highlight_pseudocode(1, true);
            UIHandler.Instance.show_message("Queue is empty!");
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, false);
        }
        else
        {
            highlight_pseudocode(1, true);
            view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = red_cell;
            yield return new WaitForSeconds(speed);
            view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = green_cell;
            highlight_pseudocode(1, false);
        }
        GameHandler.Instance.algorithm_running = false;
    }
}
