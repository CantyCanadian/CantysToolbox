using System;
using System.Collections.Generic;
using Canty;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Canty
{
    /// <summary>
    /// A replacement for the default toggle group that never works as expected. This one works by just passing the toggles as inspector argument.
    /// </summary>
    public class BetterToggleGroup : MonoBehaviour
    {
        [SerializeField] private List<BetterToggle> m_Toggles = null;
        public bool RequiresSelectedChoice = true;

        [Space(16.0f)]
        public ToggleEvent OnValueChanged;
        public IntEvent OnIndexValueChanged;

        public BetterToggle ActiveToggle { get; private set; }

        private void OnValueChangedListener(BetterToggle toggle, bool value)
        {
            if (value)
            {
                ActiveToggle = toggle;

                for (int i = 0; i < m_Toggles.Count; i++)
                {
                    if (m_Toggles[i] != toggle)
                    {
                        m_Toggles[i].isOn = false;
                    }
                    else
                    {
                        OnIndexValueChanged?.Invoke(i);
                    }
                }
            }
        }

        private void Start()
        {
            for (int i = 0; i < m_Toggles.Count; i++)
            {
                BetterToggle toggle = m_Toggles[i];
                toggle.OnValueChanged.AddListener((b) => { OnValueChangedListener(toggle, b); });

                if (toggle.isInteractable)
                {
                    if (ActiveToggle != null)
                    {
                        toggle.isOn = false;
                    }
                    else
                    {
                        toggle.isOn = true;
                    }
                }
            }
        }

        [Serializable] public class ToggleEvent : UnityEvent<BetterToggle> { }
        [Serializable] public class IntEvent : UnityEvent<int> { }
    }
}