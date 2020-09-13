using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
public class ShowUI : MonoBehaviour
{
    private GameObject d_structure = null;
    private GameObject options = null;

    private RectTransform target = null;

    private static Vector3 scale_up = new Vector3(1, 1, 0);
    private static Vector3 scale_down = new Vector3(.1f, .1f, 0);

    [SerializeField] private RectTransform[] actions = null;
    [SerializeField] private RectTransform[] structures = null;

    [SerializeField] private List<string> structures_tags = null;


    public void close(RectTransform[] panel)
    {
        for (int i = 0; i < panel.Length; i++)
        {
            if (panel[i].gameObject.activeSelf)
            {
                panel[i].transform.localScale = scale_down;
                panel[i].gameObject.SetActive(false);
                break;
            }
        }

    }

    public void show_method_options(RectTransform rect)
    {

        target = rect;

        if (!rect.gameObject.activeSelf)
        {
            rect.gameObject.SetActive(true);
            scale(rect, scale_up);
        }
        else
        {
            scale(rect, scale_down);
        }

    }

    public void show_structure_options(string data_structure_tag)
    {
        d_structure = GameObject.FindGameObjectWithTag(data_structure_tag);
        
        options = d_structure.transform.GetChild(1).gameObject;


        target = options.GetComponent<RectTransform>();



        if (!options.activeSelf)
        {
            close(actions);
            if (structures_tags.Contains(data_structure_tag))
            {

                close(structures);
            }
            options.SetActive(true);
            scale(target, scale_up);
        }
        else
        {
            scale(target, scale_down);
        }
    }


    private void scale(RectTransform target, Vector3 result_scale)
    {
        if (result_scale.x < 1)
        {
            target.DOScale(result_scale, duration: .05f).OnComplete(set_inactive);
        }
        else
        {
            target.DOScale(result_scale, duration: .2f);
        }
    }

    private void set_inactive()
    {
        target.gameObject.SetActive(false);
    }

}
