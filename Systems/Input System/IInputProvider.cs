using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputProvider
{
    bool GetActionDown(InputActions action);
    bool GetActionUp(InputActions action);
    bool GetAction(InputActions action);
    bool GetActionDownDoubleTapped(InputActions action);
    bool GetActionDoubleTapped(InputActions action);
}