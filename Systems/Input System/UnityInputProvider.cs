using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInputProvider : MonoBehaviour, IInputProvider
{
    [SerializeField] private float m_DoubleTapTimer = 0.3f;
    [SerializeField] private List<InputActions> m_CanDoubleTap = null;

    private Dictionary<InputActions, KeyContainer> m_KeyStates = null;

    private class KeyContainer
    {
        public static bool s_Initialized = false;

        public static float s_DoubleTapTimer;

        public enum InputDataState
        {
            Lifted,
            Up,

            Pressed,
            Down,

            DoubleTappedPressed,
            DoubleTappedDown
        }

        public InputDataState State { get; private set; }
        public KeyCode KeyToListen { get; private set; }

        private bool m_CanDoubleTap;
        private float m_DoubleTapDelta;

        public void Update(bool isDown)
        {
            if (m_CanDoubleTap && m_DoubleTapDelta > 0.0f)
            {
                m_DoubleTapDelta -= Time.deltaTime;

                if (m_DoubleTapDelta < 0.0f)
                {
                    m_DoubleTapDelta = 0.0f;
                }
            }

            if (isDown)
            {
                switch (State)
                {
                    case InputDataState.Lifted:
                    case InputDataState.Up:
                        {
                            if (m_CanDoubleTap && m_DoubleTapDelta > 0.0f)
                            {
                                m_DoubleTapDelta = 0.0f;
                                State = InputDataState.DoubleTappedPressed;
                            }
                            else
                            {
                                if (m_CanDoubleTap)
                                {
                                    m_DoubleTapDelta = s_DoubleTapTimer;
                                }

                                State = InputDataState.Pressed;
                            }
                        }
                        break;

                    case InputDataState.Pressed:
                        State = InputDataState.Down;
                        break;

                    case InputDataState.DoubleTappedPressed:
                        State = InputDataState.DoubleTappedDown;
                        break;
                }
            }
            else
            {
                switch (State)
                {
                    case InputDataState.DoubleTappedPressed:
                    case InputDataState.DoubleTappedDown:
                        State = InputDataState.Lifted;
                        break;

                    case InputDataState.Pressed:
                    case InputDataState.Down:
                        State = InputDataState.Lifted;
                        break;

                    case InputDataState.Lifted:
                        State = InputDataState.Up;
                        break;
                }
            }
        }

        public void UpdateKey(KeyCode newKey, bool canDoubleTap)
        {
            if (KeyToListen != newKey)
            {
                KeyToListen = newKey;
                State = InputDataState.Up;
            }
        }

        public KeyContainer(KeyCode keyToListen, bool canDoubleTap)
        {
            State = InputDataState.Up;
            KeyToListen = keyToListen;
            m_CanDoubleTap = canDoubleTap;

            m_DoubleTapDelta = 0.0f;
        }
    }

    public bool GetActionDown(InputActions action)
    {
        if (!m_KeyStates.ContainsKey(action))
        {
            return false;
        }

        return m_KeyStates[action].State == KeyContainer.InputDataState.Pressed || m_KeyStates[action].State == KeyContainer.InputDataState.DoubleTappedPressed;
    }

    public bool GetActionUp(InputActions action)
    {
        if (!m_KeyStates.ContainsKey(action))
        {
            return false;
        }

        return m_KeyStates[action].State == KeyContainer.InputDataState.Lifted;
    }

    public bool GetAction(InputActions action)
    {
        if (!m_KeyStates.ContainsKey(action))
        {
            return false;
        }

        KeyContainer.InputDataState state = m_KeyStates[action].State;
        return state == KeyContainer.InputDataState.Pressed || state == KeyContainer.InputDataState.Down || state == KeyContainer.InputDataState.DoubleTappedPressed || state == KeyContainer.InputDataState.DoubleTappedDown;
    }

    public bool GetActionDownDoubleTapped(InputActions action)
    {
        if (!m_KeyStates.ContainsKey(action))
        {
            return false;
        }

        return m_KeyStates[action].State == KeyContainer.InputDataState.DoubleTappedPressed;
    }

    public bool GetActionDoubleTapped(InputActions action)
    {
        if (!m_KeyStates.ContainsKey(action))
        {
            return false;
        }

        return m_KeyStates[action].State == KeyContainer.InputDataState.DoubleTappedDown || m_KeyStates[action].State == KeyContainer.InputDataState.DoubleTappedPressed;
    }

    private void OnActionToKeysChanged()
    {
        InputManager.ActionToKeyDictionary newMap = InputManager.Instance.CurrentActionToKey;

        m_KeyStates.Clear();

        foreach (KeyValuePair<InputActions, KeyCode> kvp in InputManager.Instance.CurrentActionToKey)
        {
            bool canDoubleTap = m_CanDoubleTap.Contains(kvp.Key);
            m_KeyStates.Add(kvp.Key, new KeyContainer(kvp.Value, canDoubleTap));
        }
    }

    private void Update()
    {
        foreach (KeyValuePair<InputActions, KeyContainer> kvp in m_KeyStates)
        {
            bool key = Input.GetKey(kvp.Value.KeyToListen);
            kvp.Value.Update(key);
        }
    }

    private void Start()
    {
        if (!KeyContainer.s_Initialized)
        {
            KeyContainer.s_Initialized = true;

            KeyContainer.s_DoubleTapTimer = m_DoubleTapTimer;
        }

        m_KeyStates = new Dictionary<InputActions, KeyContainer>();

        InputManager.Instance.OnActionToKeysChanged += OnActionToKeysChanged;
        OnActionToKeysChanged();
    }
}
