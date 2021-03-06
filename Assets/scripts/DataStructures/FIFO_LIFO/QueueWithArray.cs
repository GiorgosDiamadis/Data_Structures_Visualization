﻿using DG.Tweening;
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

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, size: new Vector2(100, 100));
    }

    public override void Stop_Execution()
    {
        base.Stop_Execution();

        for (int i = 0; i < view.transform.childCount; i++)
        {
                view.transform.Get_Component_In_Child<Image>(i, 0).sprite = green_cell;
        }
    }
    public void Dequeue()
    {
        if (GameHandler.Instance.algorithm_running)
            return;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(0.001f, 0.001f, 0.001f), duration: .2f);

        GameHandler.Instance.algorithm_running = true;
        StartCoroutine(dequeue_cor());
    }

    private IEnumerator dequeue_cor()
    {
        UIHandler.Instance.close_message();
      

        if (view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0,0,0).text == string.Empty)
        {

            Load_Pseudocode("dequeue");
            yield return new WaitForSeconds(0.5f);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            UIHandler.Instance.show_message("Queue is empty!");
        }
        else
        {
            Load_Pseudocode("dequeue");
            UIHandler.Instance.UXinfo("Dequeueing " + view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0, 0).text, true);

            yield return new WaitForSeconds(0.5f);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);


            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = red_cell;
            highlight_pseudocode(1, true);

            yield return StartCoroutine(Wait());

            highlight_pseudocode(1, false);
            view.transform.Get_Component_In_Child<Image>(0,0).sprite = green_cell;
            view.transform.GetChild(0).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = string.Empty;
            next_enqueue--;

            for (int i = 1; i < view.transform.childCount; i++)
            {
                view.transform.GetChild(i).SetSiblingIndex(i - 1);
            }
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);
        GameHandler.Instance.algorithm_running = false;
        UIHandler.Instance.UXinfo("", false);

    }

    public IEnumerator Enqueue(long data)
    {
        UIHandler.Instance.close_message();
        UIHandler.Instance.UXinfo("Enqueueing " + data,true);
        Load_Pseudocode("enqueue");
        yield return new WaitForSeconds(0.5f);

        highlight_pseudocode(0, true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, false);

        if (next_enqueue == 10)
        {
            UIHandler.Instance.show_message("Queue is full!");
        }
        else
        {

            view.transform.Get_Component_In_Child<Image>(next_enqueue,0).sprite = red_cell;
            highlight_pseudocode(1, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, false);
            view.transform.Get_Component_In_Child<Image>(next_enqueue, 0).sprite = green_cell;
            view.transform.GetChild(next_enqueue).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.ToString();

            next_enqueue++;
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);
        GameHandler.Instance.algorithm_running = false;
        UIHandler.Instance.UXinfo("" ,false);

    }


    public void Peek()
    {
        if (GameHandler.Instance.algorithm_running)
            return;

        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(0.001f, 0.001f, 0.001f), duration: .2f);
        GameHandler.Instance.algorithm_running = true;

        StartCoroutine(peek_cor());
    }

    private IEnumerator peek_cor()
    {
        UIHandler.Instance.close_message();

        

        if (view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0, 0).text == string.Empty)
        {
            Load_Pseudocode("peek");
            yield return new WaitForSeconds(0.5f);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);
            UIHandler.Instance.show_message("Queue is empty!");
        }
        else
        {
            Load_Pseudocode("peek");
            UIHandler.Instance.UXinfo("Peeking " + view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0, 0).text, true);
            yield return new WaitForSeconds(0.5f);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            highlight_pseudocode(1, true);
            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = red_cell;
            yield return StartCoroutine(Wait());
            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = green_cell;
            highlight_pseudocode(1, false);
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);
        GameHandler.Instance.algorithm_running = false;
        UIHandler.Instance.UXinfo("", false);

    }
}
