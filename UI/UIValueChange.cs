using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIValueChange : MonoBehaviour
{
    public Text UIText;

    public string TrueText;
    public string FalseText;

    public void OnValueChanged(bool flag)
    {
        UIText.text = flag ? TrueText : FalseText;
    }

    public void OnValueChanged(float value)
    {
        UIText.text = value.ToString();
    }

    public void OnValueChanged(string value)
    {
        UIText.text = value;
    }

    public void OnValueChanged(Dropdown origin)
    {
        UIText.text = origin.options[origin.value].text;
    }
}
