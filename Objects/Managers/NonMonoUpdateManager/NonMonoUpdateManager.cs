using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMonoUpdateManager : Singleton<NonMonoUpdateManager>
{
    private List<UpdateableBase> m_Updateables;

    public void RegisterUpdateable(UpdateableBase updateable)
    {
        if (m_Updateables == null)
        {
            m_Updateables = new List<UpdateableBase>();
        }
        else if (m_Updateables.Contains(updateable))
        {
            Debug.LogWarning("NonMonoUpdateManager : Trying to add the same object to the manager multiple time.");
            return;
        }

        m_Updateables.Add(updateable);
    }

    private void Update()
    {
        if (m_Updateables.Count > 0)
        {
            for (int i = 0; i < m_Updateables.Count; i++)
            {
                if (m_Updateables[i] == null)
                {
                    m_Updateables.RemoveAt(i);
                    i--;
                    continue;
                }

                m_Updateables[i].Update();
            }
        }
    }
}
