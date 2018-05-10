using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolData : MonoBehaviour
{
    public string PoolName;
    public Text AvailableAndUsed;
    public Text Total;

	void Update ()
    {
        Vector3 data = PoolManager.Instance.GetPoolCounts(PoolName);

        AvailableAndUsed.text = data.x.ToString() + " / " + data.y.ToString();
        Total.text = data.z.ToString();
	}
}
