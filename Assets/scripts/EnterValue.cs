﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnterValue : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMPro.TMP_InputField input_field;
    [SerializeField] private TMPro.TextMeshProUGUI data;
    public void OnPointerClick(PointerEventData eventData)
    {
        input_field.gameObject.SetActive(true);
    }


    public void Apply_Value()
    {
        long value = long.Parse(input_field.text);
        data.text = value.ToString();
        print(data.text);
        print(value);
        input_field.gameObject.SetActive(false);

    }
}
