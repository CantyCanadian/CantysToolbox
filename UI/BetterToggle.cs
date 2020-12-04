using System;
using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BetterToggle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private enum BetterToggleState
    {
        Null,

        Enabled,
        Hovered,
        Active,
        Disabled
    }

    public bool StartAsOn = false;
    public bool StartAsInteractable = true;

    public Graphic TargetGraphic;

    public Color32 EnabledColor = new Color32(255, 255, 255, 255);
    public Color32 HoveredColor = new Color32(245, 245, 245, 255);
    public Color32 ActiveColor = new Color32(166, 166, 166, 255);
    public Color32 DisabledColor = new Color32(200, 200, 200, 128);

    [Space(8.0f)]
    public BoolEvent OnValueChanged;

    private BetterToggleGroup m_Group = null;

    private BetterToggleState m_State = BetterToggleState.Null;
    private bool m_MouseInside = false;

    public bool isOn
    {
        get { return m_State == BetterToggleState.Active; }
        set
        {
            if (m_State == BetterToggleState.Disabled)
            {
                return;
            }
            else if (m_State == BetterToggleState.Active && value)
            {
                return;
            }
            else if ((m_State == BetterToggleState.Enabled || m_State == BetterToggleState.Hovered) && !value)
            {
                return;
            }

            m_State = value ? BetterToggleState.Active : (m_MouseInside ? BetterToggleState.Hovered : BetterToggleState.Enabled);
            OnValueChanged?.Invoke(value);
            m_Group?.OnValueChanged?.Invoke(this);
            RefreshUI();
        }
    }

    public bool isInteractable
    {
        get { return m_State != BetterToggleState.Disabled; }
        set
        {
            if (value)
            {
                m_State = m_MouseInside ? BetterToggleState.Hovered : BetterToggleState.Enabled;
            }
            else
            {
                m_State = BetterToggleState.Disabled;
            }

            RefreshUI();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_Group)
        {
            if (isOn)
            {
                if (!m_Group.RequiresSelectedChoice)
                {
                    isOn = false;
                }
            }
            else
            {
                isOn = true;
            }
        }
        else
        {
            isOn = !isOn;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_State == BetterToggleState.Enabled)
        {
            m_State = BetterToggleState.Hovered;
        }

        m_MouseInside = true;

        RefreshUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_State == BetterToggleState.Hovered)
        {
            m_State = BetterToggleState.Enabled;
        }

        m_MouseInside = false;

        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (m_State)
        {
            case BetterToggleState.Enabled:
                if (TargetGraphic)
                {
                    TargetGraphic.color = EnabledColor;
                }
                break;

            case BetterToggleState.Hovered:
                if (TargetGraphic)
                {
                    TargetGraphic.color = HoveredColor;
                }
                break;

            case BetterToggleState.Active:
                if (TargetGraphic)
                {
                    TargetGraphic.color = ActiveColor;
                }
                break;

            case BetterToggleState.Disabled:
                if (TargetGraphic)
                {
                    TargetGraphic.color = DisabledColor;
                }
                break;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
        {
            if (isOn != StartAsOn)
            {
                m_State = isOn ? BetterToggleState.Active : BetterToggleState.Enabled;
                isOn = StartAsOn;
            }

            if (isInteractable != StartAsInteractable)
            {
                m_State = isInteractable ? BetterToggleState.Enabled : BetterToggleState.Disabled;
                isInteractable = StartAsInteractable;
            }
        }
    }
#endif

    private void Awake()
    {
        isOn = StartAsOn;
        isInteractable = StartAsInteractable;
    }

    [Serializable] public class BoolEvent : UnityEvent<bool> { }
}
