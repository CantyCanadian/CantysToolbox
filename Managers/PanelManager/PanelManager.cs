using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Canty.Managers
{
    public class PanelManager : Singleton<PanelManager>
    {
        public delegate void PopupResult<I>(I returnValue);

        private GameObject m_ActivePopup;
        private List<GameObject> m_PanelStack;

        private Dictionary<Type, GameObject> m_RegisteredPanels = null;
        private Dictionary<Type, KeyValuePair<GameObject, Type>> m_RegisteredPopups = null;

        /// <summary>
        /// Register panel prefab and ties it to its IPanel type. Panels are regular windows.
        /// </summary>
        /// <typeparam name="I">IPanel child type.</typeparam>
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

        /// <summary>
        /// Register popup prefab and ties it to its PopupBase type. Popups are windows that returns a value when closed.
        /// </summary>
        /// <typeparam name="I">PopupBase child type.</typeparam>
        /// <typeparam name="R">Popup return type.</typeparam>
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

        /// <summary>
        /// Load panel using the IPanel type.
        /// </summary>
        /// <typeparam name="I">IPanel child type to load.</typeparam>
        public void LoadPanel<I>() where I : IPanel
        {
            if (m_RegisteredPanels.ContainsKey(typeof(I)))
            {
                m_PanelStack.DoOnAll((obj) => { obj.SetActive(false); });
                m_PanelStack.Add(Instantiate(m_RegisteredPanels[typeof(I)], transform));
            }
        }

        /// <summary>
        /// Load popup using the PopupBase type, return type and the callback function for when the popup is closed.
        /// </summary>
        /// <typeparam name="I">PopupBase child type.</typeparam>
        /// <typeparam name="R">Popup return type.</typeparam>
        public void LoadPopup<I, R>(UnityAction<R> resultReceiver)
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
                    PopupBase<R> pb = m_ActivePopup.GetComponent<PopupBase<R>>();
                    pb.OnSubmit.AddListener(PopPopup);
                    pb.OnSubmitWithData.AddListener(resultReceiver);
                }
            }
        }

        /// <summary>
        /// Pop the panel on top of the stack.
        /// </summary>
        public void PopPanel()
        {
            GameObject go = m_PanelStack.Pop();

            if (go != null)
            {
                Destroy(go);
            }
        }

        /// <summary>
        /// Pop the popup currently loaded. Shouldn't be called manually since it is called by the popup's OnSubmit callback.
        /// </summary>
        public void PopPopup()
        {
            if (m_ActivePopup != null)
            {
                Destroy(m_ActivePopup.gameObject);
            }

            m_ActivePopup = null;
        }
    }
}