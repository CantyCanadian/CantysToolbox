using Canty.Managers;
using UnityEngine;

public class ScreenshakeLink : MonoBehaviour
{
    public float TimeToShake = 2.0f;

    private float m_Delta = 2.1f;

	void Update ()
	{
	    if (m_Delta > TimeToShake)
	    {
	        ScreenShakeManager.Instance.ShakeOnce(gameObject, 0.2f, 0.0f, TimeToShake / 4.0f, TimeToShake / 2.0f, TimeToShake / 4.0f);
	        m_Delta = 0.0f;
	    }
	    else
	    {
	        m_Delta += Time.deltaTime;
	    }
	}

    void Start()
    {
        m_Delta = TimeToShake + 0.1f;
    }
}
