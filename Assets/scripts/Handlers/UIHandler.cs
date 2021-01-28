using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;
    public static GameObject structure_variant = null;
    public static GameObject selected_structure = null;
    public static GameObject method_options = null;
    public GameObject divider;
    private RectTransform target = null;
    public TMPro.TextMeshProUGUI ux;
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
        if (!GameHandler.Instance.algorithm_running)
        {

        GameObject structure = GameObject.FindWithTag(structure_tag);
        GameObject structure_panel = structure.transform.parent.Get_Child(structure.transform.GetSiblingIndex() + 1).gameObject;

        if(selected_structure == null)
        {
            selected_structure = structure_panel;
            selected_structure.SetActive(true);
            scale(selected_structure.GetComponent<RectTransform>(), scale_up);
        }
        else if (selected_structure == structure_panel)
        {
            scale(selected_structure.GetComponent<RectTransform>(), scale_down);
            selected_structure = null;
        }
        else
        {
            selected_structure.SetActive(false);
            selected_structure.transform.localScale = scale_down;

            selected_structure = structure_panel;
            selected_structure.SetActive(true);
            scale(selected_structure.GetComponent<RectTransform>(), scale_up);
        }

        if (structure_variant != null)
        {
            structure_variant.SetActive(false);
            structure_variant.transform.localScale = scale_down;
            structure_variant = null;
        }

        if(method_options != null)
        {
            method_options.SetActive(false);
            method_options.transform.localScale = scale_down;
            method_options = null;
        }

        foreach(UIGraphics g in FindObjectsOfType<UIGraphics>())
        {
            g.GetComponent<Image>().color = g.normal;
        }
        }
        
    }
    private void Show_Data_Structure_Variant_Methods(IDataStructure obj)
    {
        if (structure_variant != null)
        {
            if (structure_variant.GetComponentInParent<IDataStructure>() == obj)
            {
                RectTransform rect = structure_variant.GetComponent<RectTransform>();
                scale(rect, scale_down);
                target = rect;
                structure_variant = null;
                return;
            }
            else
            {
                structure_variant.SetActive(false);
                structure_variant.transform.localScale = scale_down;
            }
        }

        structure_variant = obj.transform.GetChild(1).gameObject;
        if(structure_variant.name != "Text (TMP)")
        {
            structure_variant.SetActive(true);
            scale(structure_variant.GetComponent<RectTransform>(), scale_up);
        }
        else
        {
            structure_variant = null;
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
        if (!GameHandler.Instance.algorithm_running)
        {
            target = rect;

            if (!rect.gameObject.activeSelf)
            {
                rect.gameObject.SetActive(true);
                method_options = rect.gameObject;
                scale(rect, scale_up);
            }
            else
            {
                method_options = null;

                scale(rect, scale_down);
            }
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
    
    public void UXinfo(string message,bool open)
    {
        ux.gameObject.SetActive(open);
        ux.text = message;
    }

    public void Structure_Deselected()
    {
        structure_variant = null;
        method_options = null;
        selected_structure = null;
        target = null;
    }
}
