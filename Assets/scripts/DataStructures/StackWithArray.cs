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

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, new Vector2(30f, 30f));

    }


    public void peek()
    {
        if (GameHandler.Instance.is_running)
            return;
        GameHandler.Instance.is_running = true;

        StartCoroutine(peek_cor());
    }

    private IEnumerator peek_cor()
    {
        UIHandler.Instance.close_message();

        Load_Pseudocode("peek");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        if (next_empty == 0)
        {
            UIHandler.Instance.show_message("Stack is empty!");
        }
        else
        {
            highlight_pseudocode(1, true);
            view.transform.GetChild(next_empty - 1).GetComponentInChildren<SpriteRenderer>().sprite = red_cell;
            yield return new WaitForSeconds(speed);
            view.transform.GetChild(next_empty - 1).GetComponentInChildren<SpriteRenderer>().sprite = green_cell;
            highlight_pseudocode(1, false);
        }
        GameHandler.Instance.is_running = false;
    }

    public IEnumerator push(long data)
    {
        UIHandler.Instance.close_message();


        Load_Pseudocode("push");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        if (next_empty == 10)
        {
            UIHandler.Instance.show_message("Stack is full!");
            highlight_pseudocode(2, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, false);
        }
        else
        {

            view.transform.GetChild(next_empty).GetComponentInChildren<SpriteRenderer>().sprite = red_cell;
            highlight_pseudocode(1, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, false);
            view.transform.GetChild(next_empty).GetComponentInChildren<SpriteRenderer>().sprite = green_cell;
            view.transform.GetChild(next_empty).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.ToString();

            next_empty++;
        }
        GameHandler.Instance.is_running = false;
    }

    public void pop()
    {
        if (GameHandler.Instance.is_running)
            return;
        GameHandler.Instance.is_running = true;
        StartCoroutine(pop_cor());
    }

    private IEnumerator pop_cor()
    {
        UIHandler.Instance.close_message();
        Load_Pseudocode("pop");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        if (next_empty == 0)
        {
            UIHandler.Instance.show_message("Stack is empty!");
            highlight_pseudocode(2, true);
            yield return new WaitForSeconds(speed);

            highlight_pseudocode(2, false);
        }
        else
        {

            highlight_pseudocode(1, true);
            next_empty--;
            view.transform.GetChild(next_empty).GetComponentInChildren<SpriteRenderer>().sprite = red_cell;

            yield return new WaitForSeconds(speed);

            highlight_pseudocode(1, false);
            view.transform.GetChild(next_empty).GetComponentInChildren<SpriteRenderer>().sprite = green_cell;
            view.transform.GetChild(next_empty).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        }

        GameHandler.Instance.is_running = false;

    }

}
