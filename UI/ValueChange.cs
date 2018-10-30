///====================================================================================================
///
///     ValueChange by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Canty.UI
{
    /// <summary>
    /// Basic class that supports all default UI callbacks for quick editing.
    /// </summary>
    public class ValueChange : MonoBehaviour
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
}