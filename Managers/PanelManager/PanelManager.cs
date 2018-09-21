using System;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : Singleton<PanelManager>
{
    public delegate void PopupResult<I>(I returnValue);

    private PopupBase m_ActivePopup;
    private List<IPanel> m_PanelStack;

    private Dictionary<Type, GameObject> m_RegisteredPanels = null;
    private Dictionary<Type, KeyValuePair<GameObject, Type>> m_RegisteredPopups = null;

    public void RegisterPanel<I>(GameObject prefab) where I : IPanel
    {
        if (m_RegisteredPanels == null)
        {
            m_RegisteredPanels = new Dictionary<Type, GameObject>();
        }

        if (!m_RegisteredPanels.ContainsKey(typeof(I)))
        {
            m_RegisteredPanels.Add(typeof(I), prefab);
        }
    }

    public void RegisterPopup<I, R>(GameObject prefab) where I : PopupBase
    {
        if (m_RegisteredPopups == null)
        {
            m_RegisteredPopups = new Dictionary<Type, KeyValuePair<GameObject, Type>>();
        }

        if (!m_RegisteredPopups.ContainsKey(typeof(I)))
        {
            m_RegisteredPopups.Add(typeof(I), new KeyValuePair<GameObject, Type>(prefab, typeof(R)));
        }
    }

    public void LoadPanel<I>() where I : IPanel
    {
        if (m_RegisteredPanels.ContainsKey(typeof(I)))
        {
            m_PanelStack.DoOnAll((obj) => { obj.gameObject.SetActive(false); });
            m_PanelStack.Add(Instantiate(m_RegisteredPanels[typeof(I)], transform));
        }
    }

    public void LoadPopup<I, R>(PopupResult<R> resultReceiver)
    {
        if (m_RegisteredPopups.ContainsKey(typeof(I)))
        {
            if (m_RegisteredPopups[typeof(I)].Value == typeof(R))
            {
                if (m_ActivePopup != null)
                {
                    Destroy(m_ActivePopup.gameObject);
                }

                m_ActivePopup = Instantiate(m_RegisteredPopups[typeof(I)].Key, transform);
                PopupBase pb = m_ActivePopup.GetComponent<PopupBase>();
                pb.OnSubmit += PopPopup();
                pb.OnSubmit += resultReceiver;
            }
        }
    }

    public void PopPanel()
    {
        GameObject go = m_PanelStack.Pop();

        if (go != null)
        {
            Destroy(go);
        }
    }

    public void PopPopup()
    {
        if (m_ActivePopup != null)
        {
            Destroy(m_ActivePopup.gameObject);
        }

        m_ActivePopup = null;
    }
}
