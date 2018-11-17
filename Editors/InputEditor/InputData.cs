using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty.Input
{
    public class InputData : ScriptableObject
    {
        private List<string> m_Styles;

        public string[] GetStyles()
        {
            return m_Styles.ToArray();
        }
    }
}