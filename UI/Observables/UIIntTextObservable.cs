using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Text))]
public class UIIntTextObservable : MonoBehaviour
{
    public IntObservable ValueToObserve;

    private Text m_Text = null;

	void Update ()
    {
		if (m_Text == null)
        {
            m_Text = GetComponent<Text>();
        }

        m_Text.text = ValueToObserve.Invoke().ToString();
	}
}

[System.Serializable]
public class IntObservable : SerializableCallback<int> { }