using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimatorController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Animator animator;
    public string current;
    private AnimatorController[] all;
    private void Start()
    {
        animator = GetComponent<Animator>();
        all = FindObjectsOfType<AnimatorController>();
        current = "normal";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (current == "normal")
        {
            foreach (AnimatorController a in all)
            {
                if (a != this)
                {
                    if (a.current == "highlight")
                    {
                        a.animator.SetTrigger("normal");
                        a.current = "normal";
                    }
                }

            }

            animator.SetTrigger("highlight");
            current = "highlight";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (current == "highlight")
        {
            animator.SetTrigger("normal");
            current = "normal";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if(current == "selected")
        {
            animator.SetTrigger("normal");
            current = "normal";
        }
        else
        {
            foreach (AnimatorController a in all)
            {
                if (a != this)
                {
                    a.animator.SetTrigger("normal");
                    a.current = "normal";
                }
            }

            animator.SetTrigger("selected");
            current = "selected";
        }
        
    }
}
