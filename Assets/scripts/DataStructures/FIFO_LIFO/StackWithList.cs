using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class StackWithList : IDataStructure, IStack
{
    public override void Stop_Execution()
    {
        base.Stop_Execution();

        if (UIHandler.Instance.ux.transform.childCount != 0)
        {
            UIHandler.Instance.ux.transform.Destroy_All_Children();
        }

        for (int i = 0; i < view.transform.childCount; i++)
        {
            if (view.transform.GetChild(i).tag == "Node")
            {
                view.transform.Get_Component_In_Child<Image>(i, 0).sprite = initial_sprite;
            }
        }
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

        ViewHandler.Instance.Change_Grid(GridLayoutGroup.Axis.Vertical, GridLayoutGroup.Constraint.FixedRowCount, 1,size:new Vector2(100,100));

        max_counter = 3;
        max_nodes = 14;
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

        

        if (view.transform.childCount == 0)
        {
            Load_Pseudocode("peek");
            yield return new WaitForSeconds(0.5f);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            
            highlight_pseudocode(2, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(2, false);

            UIHandler.Instance.show_message("Stack is empty!");

        }
        else
        {
            
            Load_Pseudocode("peek");
            yield return new WaitForSeconds(0.5f);
            UIHandler.Instance.UXinfo("Peeking " + view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(view.transform.childCount - 1, 0, 0).text, true);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);

            view.transform.Get_Component_In_Child<Image>(view.transform.childCount - 1, 0).sprite = traverse_sprite;

            highlight_pseudocode(1, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(1, false);

            view.transform.Get_Component_In_Child<Image>(view.transform.childCount - 1, 0).sprite = initial_sprite;
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);
        UIHandler.Instance.UXinfo("", false);

        GameHandler.Instance.algorithm_running = false;
    }

    public IEnumerator push(long data)
    {
        UIHandler.Instance.close_message();
        UIHandler.Instance.UXinfo("Pushing " + data, true);
        GameObject to_add = create_ux_node(data,UIHandler.Instance.ux.transform);

        Load_Pseudocode("push");
        yield return new WaitForSeconds(0.5f);

        highlight_pseudocode(0, true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(0, false);

        highlight_pseudocode(1, true);
        yield return StartCoroutine(Wait());
        highlight_pseudocode(1, false);


        if (view.transform.childCount > 1)
        {
            highlight_pseudocode(3, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(3, false);

            GameObject arrow = create_arrow();
            arrow.transform.SetAsLastSibling();

            GameObject node = create_node(data);
            node.transform.SetAsLastSibling();
        }
        else
        {
            highlight_pseudocode(2, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(2, false);

            GameObject node = create_node(data);
            node.transform.SetAsLastSibling();
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);


        GameHandler.Instance.handle_insertion.Invoke();
        GameHandler.Instance.algorithm_running = false;

        ViewHandler.Instance.Change_Grid(enabled:true,size: new Vector2(100, 100));
        UIHandler.Instance.UXinfo("", false);
        to_add.Destroy_Object();

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

        if (view.transform.childCount > 0)
        {

            UIHandler.Instance.UXinfo("Popping " + view.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(view.transform.childCount - 1, 0, 0).text, true);

            Load_Pseudocode("pop");
            yield return new WaitForSeconds(0.5f);
            
            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);
            highlight_pseudocode(1, true);
            view.transform.Get_Component_In_Child<Image>(view.transform.childCount - 1, 0).sprite = traverse_sprite;

            yield return StartCoroutine(Wait());

            view.transform.Get_Component_In_Child<Image>(view.transform.childCount - 1, 0).sprite = initial_sprite;
            highlight_pseudocode(1, false);
            view.transform.Destroy_Child(view.transform.childCount - 1);

            if (view.transform.childCount > 0)
                view.transform.Destroy_Child(view.transform.childCount - 1);

            GameHandler.Instance.handle_deletion.Invoke();

        }
        else
        {
            Load_Pseudocode("pop");
            yield return new WaitForSeconds(0.5f);

            highlight_pseudocode(0, true);
            yield return StartCoroutine(Wait());
            highlight_pseudocode(0, false);
            UIHandler.Instance.show_message("Stack is empty!");
        }
        transform.Get_Component_In_Child<RectTransform>(1).DOScale(new Vector3(1f, 1f, 1f), duration: .2f);

        GameHandler.Instance.algorithm_running = false;
        UIHandler.Instance.UXinfo("", false);

    }

}
