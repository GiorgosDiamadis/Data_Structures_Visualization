using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class StackWithList : IDataStructure, IStack
{

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


        view.GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Horizontal;
        view.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        view.GetComponent<GridLayoutGroup>().constraintCount = 1;
        view.GetComponent<GridLayoutGroup>().spacing = new Vector2(10f, 10f);

        max_counter = 2;
        max_nodes = 12;
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

        if (view.transform.childCount == 0)
        {
            UIHandler.Instance.show_message("Stack is empty!");
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

    public IEnumerator push(long data)
    {
        UIHandler.Instance.close_message();

        Load_Pseudocode("push");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        highlight_pseudocode(1, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(1, false);


        if (view.transform.childCount > 0)
        {
            highlight_pseudocode(3, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(3, false);

            GameObject arrow = create_arrow();
            arrow.transform.SetAsFirstSibling();

            GameObject node = create_node(data);
            node.transform.SetAsFirstSibling();
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
            UIHandler.Instance.show_message("Stack is empty!");

            highlight_pseudocode(2, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(2, false);
        }

        GameHandler.Instance.is_running = false;
    }

}
