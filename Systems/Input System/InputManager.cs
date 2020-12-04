using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Canty;

public enum InputActions
{
    // Sidescroller Input

    MoveLeft,
    MoveRight,
    Jump,
    Up,
    Down,


    // Top-Down Input

    // Up,
    // Down,
    Left,
    Right,


    // Specific Input

    Warp,
    UseAbility,
    CancelAbility,

    Ability1,   // Example = 1
    Ability2,   // Example = 2
    Ability3,   // Example = 3
    Ability4,   // Example = 4
    Ability5,   // Example = Q
    Ability6,   // Example = E
    Ability7,   // Example = R
    Ability8,   // Example = F
    Ability9,   // Example = Ins
    Ability10,  // Example = Home
    Ability11,  // Example = Pg Up
    Ability12,  // Example = Del
    Ability13,  // Example = End
    Ability14,  // Example = Pg Dn

    Count
}

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private ActionToKeyDictionary m_DefaultActionToKeys = null;
    public ActionToKeyDictionary CurrentActionToKey { get; private set; }
    
    public IInputProvider CurrentPlayerInputProvider { get; private set; }

    public Action OnPlayerInputProviderChanged;
    public Action OnActionToKeysChanged;

    public void ChangePlayerInputProvider(IInputProvider newInputProvider)
    {
        CurrentPlayerInputProvider = newInputProvider;
        OnPlayerInputProviderChanged?.Invoke();
    }

    public bool ChangeActionToKey(InputActions action, KeyCode newKey, bool changeOnError = true)
    {
        if (!CurrentActionToKey.ContainsKey(action))
        {
            CurrentActionToKey.Add(action, newKey);

            OnActionToKeysChanged?.Invoke();
            return true;
        }

        foreach (KeyValuePair<InputActions, KeyCode> actionKey in CurrentActionToKey)
        {
            if (actionKey.Value == newKey)
            {
                if (changeOnError)
                {
                    CurrentActionToKey[action] = newKey;
                    CurrentActionToKey[actionKey.Key] = KeyCode.None;

                    OnActionToKeysChanged?.Invoke();
                }
                
                return false;
            }
        }

        OnActionToKeysChanged?.Invoke();
        return true;
    }

    private void Awake()
    {
        CurrentActionToKey = m_DefaultActionToKeys;
        ChangePlayerInputProvider(GetComponent<IInputProvider>());
    }

    [System.Serializable] public class ActionToKeyDictionary : SerializableDictionary<InputActions, KeyCode> { }
}
