﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueWithList : IDataStructure,IQueue
{
    public void Dequeue()
    {
        if (GameHandler.Instance.is_running)
            return;
        GameHandler.Instance.is_running = true;
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

        if (view.transform.childCount > 0)
        {
            highlight_pseudocode(1, true);
            view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = traverse_sprite;

            yield return new WaitForSeconds(speed);

            view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = initial_sprite;
            highlight_pseudocode(1, false);
            view.transform.Destroy_Child(0);

            if (view.transform.childCount > 0)
                view.transform.Destroy_Child(0);

            GameHandler.Instance.handle_deletion.Invoke();

        }
        else
        {
            UIHandler.Instance.show_message("Queue is empty!");

            highlight_pseudocode(2, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, false);
        }

        GameHandler.Instance.is_running = false;
    }

    public IEnumerator Enqueue(long data)
    {
        UIHandler.Instance.close_message();

        Load_Pseudocode("enqueue");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        if (view.transform.childCount > 0)
        {
            highlight_pseudocode(1, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, false);

            GameObject arrow = create_arrow();

            GameObject node = create_node(data);
        }
        else
        {
            highlight_pseudocode(2, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, false);

            GameObject node = create_node(data);
            node.transform.SetAsFirstSibling();
        }
        GameHandler.Instance.handle_insertion.Invoke();
        GameHandler.Instance.is_running = false;
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

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1, new Vector2(10f, 10f));

        max_counter = 3;
        max_nodes = 14;
    }

    public void Peek()
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

        if (view.transform.childCount == 0)
        {
            UIHandler.Instance.show_message("Queue is empty!");
            highlight_pseudocode(2, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, false);
        }
        else
        {
            view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = traverse_sprite;
            highlight_pseudocode(1, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, false);
            view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = initial_sprite;
        }
        GameHandler.Instance.is_running = false;
    }
}
