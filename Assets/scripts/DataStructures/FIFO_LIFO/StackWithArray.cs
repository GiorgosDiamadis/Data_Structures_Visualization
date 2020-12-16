using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackWithArray : IDataStructure, IStack
{

    private int next_empty = -1;

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

        next_empty = 3;
        max_nodes = 10;

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, size: new Vector2(100, 100));

    }


    public void peek()
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
       

        if (next_empty == 0)
        {
            Load_Pseudocode("peek");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            UIHandler.Instance.show_message("Stack is empty!");
        }
        else
        {
            UIHandler.Instance.UXinfo("Peeking " + view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0, 0).text, true);

            Load_Pseudocode("peek");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            highlight_pseudocode(1, true);
            view.transform.Get_Component_In_Child<Image>(next_empty-1, 0).sprite = red_cell;
            yield return StartCoroutine(Wait());
            view.transform.Get_Component_In_Child<Image>(next_empty-1, 0).sprite = green_cell;
            highlight_pseudocode(1, false);
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.algorithm_running = false;
        UIHandler.Instance.UXinfo("", false);
    }

    public IEnumerator push(long data)
    {
        UIHandler.Instance.close_message();

        UIHandler.Instance.UXinfo("Pushing " + data, true);

        Load_Pseudocode("push");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, false);

        if (next_empty == 10)
        {
            UIHandler.Instance.show_message("Stack is full!");
        }
        else
        {

            view.transform.Get_Component_In_Child<Image>(next_empty, 0).sprite = red_cell;
            highlight_pseudocode(1, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, false);
            view.transform.Get_Component_In_Child<Image>(next_empty, 0).sprite = green_cell;
            view.transform.GetChild(next_empty).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.ToString();

            next_empty++;
        }

        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);
        GameHandler.Instance.algorithm_running = false;
            UIHandler.Instance.UXinfo("", false);

    }

    public void pop()
    {
        if (GameHandler.Instance.algorithm_running)
            return;
        GameHandler.Instance.algorithm_running = true;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

        StartCoroutine(pop_cor());
    }

    private IEnumerator pop_cor()
    {
        UIHandler.Instance.close_message();
       

        if (next_empty == 0)
        {
            Load_Pseudocode("pop");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            UIHandler.Instance.show_message("Stack is empty!");
        }
        else
        {
            UIHandler.Instance.UXinfo("Popping " + view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(next_empty-1, 0, 0).text, true);

            Load_Pseudocode("pop");
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            highlight_pseudocode(1, true);
            next_empty--;
            view.transform.Get_Component_In_Child<Image>(next_empty, 0).sprite = red_cell;

            yield return StartCoroutine(Wait());

            highlight_pseudocode(1, false);
            view.transform.Get_Component_In_Child<Image>(next_empty,0).sprite = green_cell;
            view.transform.GetChild(next_empty).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        }

        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.algorithm_running = false;
        UIHandler.Instance.UXinfo("", false);
    }

}
