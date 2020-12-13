using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using DG.Tweening;

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

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Horizontal, GridLayoutGroup.Constraint.FixedColumnCount, 1,size:new Vector2(75,75));

        max_counter = 2;
        max_nodes = 8;
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
            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = traverse_sprite;
            highlight_pseudocode(1, true);
            yield return new WaitForSeconds(speed);
            highlight_pseudocode(1, false);
            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = initial_sprite;
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.algorithm_running = false;
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
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.handle_insertion.Invoke();
        GameHandler.Instance.algorithm_running = false;
    }

    public void pop()
    {
        if (GameHandler.Instance.algorithm_running)
            return;
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(.001f, .001f, .001f), duration: .2f);

        GameHandler.Instance.algorithm_running = true;
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
            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = traverse_sprite;

            yield return new WaitForSeconds(speed);

            view.transform.Get_Component_In_Child<Image>(0, 0).sprite = initial_sprite;
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
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.algorithm_running = false;
    }

}
