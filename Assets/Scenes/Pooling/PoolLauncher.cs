using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLauncher : MonoBehaviour
{
    public GameObject OriginalPrefab;

    public float TimeBetweenLaunches = 0.2f;

    public bool DiscardAfterTime = false;
    public float DiscardTime = 5.0f;

    public int Count = 100;
    public int MaximumCount = 100;

    private PoolManager m_PoolManagerInstance = null;

	void Start ()
    {
        m_PoolManagerInstance = PoolManager.Instance;

        m_PoolManagerInstance.CreatePool("Launcher", OriginalPrefab, Count, MaximumCount, DiscardAfterTime ? DiscardTime : -1.0f);

        StartCoroutine(LauncherLoop());
	}
	
	private IEnumerator LauncherLoop()
    {
        while(true)
        {
            GameObject go = m_PoolManagerInstance.GetObject("Launcher");
            go.SetActive(true);
            go.transform.position = transform.position + new Vector3(0.0f, 1.0f, 0.0f);

            go.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 30.0f), Random.Range(-1.0f, 1.0f)), transform.position, ForceMode.VelocityChange);

            yield return new WaitForSeconds(TimeBetweenLaunches);
        }
    }
}
