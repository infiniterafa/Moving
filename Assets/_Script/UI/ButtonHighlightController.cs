using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// La lógica que modifica el color del botón y el tamaño que nos da el feedback de la usabilidad

public class ButtonHighlightController : MonoBehaviour
{
    [SerializeField]
    private List<ButtonFeedback> buttons = new();

    [SerializeField]
    AudioSource buttonClickSound;

    private void Awake()
    {
        if(buttons.Count == 0)
            buttons = new(GetComponentsInChildren<ButtonFeedback>());
        foreach(var button in buttons)
        {
            button.OnClicked += SelectionFeedback;
        }
    }

    public void ResetAll()
    {
        foreach (var button in buttons)
        {
            button.ResetButton();
        }
    }

    private void SelectionFeedback()
    {
        ResetAll();
        buttonClickSound.Play();
    }
}
