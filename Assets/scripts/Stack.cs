using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Stack : IDataStructure
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
        num_nodes = 2;
        max_nodes = 12;
    }

    public void peek()
    {
        if (view.transform.childCount == 0)
        {
            UIHandler.Instance.show_message("Stack is empty!");
            return;
        }

        StartCoroutine(peek_cor());
    }

    private IEnumerator peek_cor()
    {
        Load_Pseudocode("peek");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(0, false);

        view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = traverse_sprite;
        highlight_pseudocode(1, true);
        yield return new WaitForSeconds(speed);
        highlight_pseudocode(1, false);
        view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = initial_sprite;
    }

    public IEnumerator push(long data)
    {
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
            GameObject arrow = create_arrow();
            arrow.transform.SetAsFirstSibling();
        }

        GameObject node = create_node(data);
        node.transform.SetAsFirstSibling();
        
        GameHandler.Instance.handle_insertion.Invoke();
    }

    public void pop()
    {
        if(view.transform.childCount == 0)
        {
            UIHandler.Instance.show_message("Stack is empty!");
            return;
        }
        StartCoroutine(pop_cor());
    }

    private IEnumerator pop_cor()
    {

        Load_Pseudocode("pop");
        yield return new WaitForSeconds(speed);

        highlight_pseudocode(0, true);
        view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = traverse_sprite;
        
        yield return new WaitForSeconds(speed);
        
        view.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = initial_sprite;
        highlight_pseudocode(0, false);

        view.transform.DestroyChild(0);
        
        if (view.transform.childCount > 0)
            view.transform.DestroyChild(0);

        GameHandler.Instance.handle_deletion.Invoke();
    }

}
