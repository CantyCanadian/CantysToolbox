using System;
using System.Collections.Generic;
using Canty;
using UnityEngine;
using UnityEngine.UI;

public class BetterButtonGroup : MonoBehaviour
{
    public bool RequiresSelectedChoice;
    [ConditionalField("RequiresSelectedChoice", true)] public bool ChoiceDisableOnOutsideClick;

    public Action<Button> OnValueChanged;
    public Action<int> OnIndexValueChanged;

    private Button m_EnabledButton;
    private List<Button> m_Buttons;

    private bool[] m_OriginalButtonState;

    private void OnToggleValueChanged(Button button)
    {
        if (m_EnabledButton != null)
        {
            m_EnabledButton.interactable = true;
        }

        button.interactable = false;
        m_EnabledButton = button;

        OnValueChanged(button);
        OnIndexValueChanged(m_Buttons.IndexOf(button));
    }

    private void Start()
    {
        m_EnabledButton = null;
        m_Buttons = new List<Button>(GetComponentsInChildren<Button>());
        m_OriginalButtonState = new bool[m_Buttons.Count];

        foreach (Button b in m_Buttons)
        {
            b.onClick.AddListener(delegate { OnToggleValueChanged(b); });
        }

        if (RequiresSelectedChoice)
        {
            m_EnabledButton = m_Buttons[0];
            m_EnabledButton.interactable = false;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!RequiresSelectedChoice && ChoiceDisableOnOutsideClick)
            {
                if (m_EnabledButton != null)
                {
                    m_EnabledButton.interactable = true;
                    m_EnabledButton = null;
                }

                OnValueChanged(null);
                OnIndexValueChanged(-1);
            }
        }
    }
}
