using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackWithArray : IDataStructure,IStack
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
        view.GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Vertical;
        view.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
        view.GetComponent<GridLayoutGroup>().constraintCount = 1;
        view.GetComponent<GridLayoutGroup>().spacing = new Vector2(20f,20f);

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
        yield return null;
        view.transform.GetChild(next_empty++).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.ToString();

    }

    public void pop()
    {
        StartCoroutine(pop_cor());
    }

    private IEnumerator pop_cor()
    {
        yield return null;
        view.transform.GetChild(--next_empty).GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
    }

}
