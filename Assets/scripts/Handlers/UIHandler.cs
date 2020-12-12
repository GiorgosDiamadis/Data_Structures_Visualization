using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;
    public static GameObject current_structure_variant_open_panel = null;
    public static GameObject current_data_structure_open_panel = null;
    public static GameObject current_method_options_open = null;
    private RectTransform target = null;

    private static Vector3 scale_up = new Vector3(1, 1, 0);
    private static Vector3 scale_down = new Vector3(.1f, .1f, 0);

    [SerializeField] private GameObject message_panel = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
        GameHandler.Instance.On_Data_Structure_Variant_Change += Show_Data_Structure_Variant_Methods;
    }
    public void Show_Data_Structure_Variants(string structure_tag)
    {
        if (current_data_structure_open_panel != null)
        {
            current_data_structure_open_panel.SetActive(false);
            current_data_structure_open_panel.transform.localScale = scale_down;
            current_data_structure_open_panel = null;
        }

        if (current_structure_variant_open_panel != null && current_structure_variant_open_panel.name!= "Text (TMP)")
        {
            current_structure_variant_open_panel.SetActive(false);
            current_structure_variant_open_panel.transform.localScale = scale_down;
            current_structure_variant_open_panel = null;
        }

        if (current_method_options_open != null)
        {
            current_method_options_open.SetActive(false);
            current_method_options_open.transform.localScale = scale_down;
            current_method_options_open = null;
        }

        GameObject o = GameObject.FindWithTag(structure_tag);
        GameObject structure = o.transform.parent.Get_Child(o.transform.GetSiblingIndex() + 1).gameObject;

        current_data_structure_open_panel = structure;
        structure.SetActive(true);
        scale(structure.GetComponent<RectTransform>(), scale_up);

        foreach(UIGraphics g in FindObjectsOfType<UIGraphics>())
        {
            if (g.transform.Get_Component_In_Child<Image>(0).color == g.highlighted)
            {
                g.transform.Get_Component_In_Child<Image>(0).color = g.normal;
            }
        }

    }
    private void Show_Data_Structure_Variant_Methods(IDataStructure obj)
    {
        if (current_structure_variant_open_panel != null)
        {
            if (current_structure_variant_open_panel.GetComponentInParent<IDataStructure>() == obj)
            {
                RectTransform rect = current_structure_variant_open_panel.GetComponent<RectTransform>();
                scale(rect, scale_down);
                target = rect;
                current_structure_variant_open_panel = null;
                return;
            }
            else
            {
                current_structure_variant_open_panel.SetActive(false);
                current_structure_variant_open_panel.transform.localScale = scale_down;
            }
        }

        current_structure_variant_open_panel = obj.transform.GetChild(1).gameObject;
        if(current_structure_variant_open_panel.name != "Text (TMP)")
        {
            current_structure_variant_open_panel.SetActive(true);
            scale(current_structure_variant_open_panel.GetComponent<RectTransform>(), scale_up);
        }
        else
        {
            current_structure_variant_open_panel = null;
        }
    }
    public void show_message(string message)
    {
        TMPro.TextMeshProUGUI mess = message_panel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        mess.text = message;

        message_panel.SetActive(true);
        message_panel.GetComponent<RectTransform>().DOScale(scale_up, .2f);
    }
    public void show_method_options(RectTransform rect)
    {
        target = rect;

        if (!rect.gameObject.activeSelf)
        {
            rect.gameObject.SetActive(true);
            current_method_options_open = rect.gameObject;
            scale(rect, scale_up);
        }
        else
        {
            current_method_options_open = null;

            scale(rect, scale_down);
        }
    }
    public void scale(RectTransform target, Vector3 result_scale)
    {
        this.target = target;
        if (result_scale.x < 1)
        {
            target.DOScale(result_scale, duration: .05f).OnComplete(set_inactive);
        }
        else
        {
            target.DOScale(result_scale, duration: .2f);
        }
    }

    public void close_message()
    {
        target = message_panel.GetComponent<RectTransform>();
        message_panel.GetComponent<RectTransform>().DOScale(scale_down, duration: .05f).OnComplete(set_inactive);
    }
    private void set_inactive()
    {
        target.gameObject.SetActive(false);
    }
}
