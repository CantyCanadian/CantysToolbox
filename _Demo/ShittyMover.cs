using UnityEngine;
using Canty;

public class ShittyMover : MonoBehaviour
{
    public CurveTimer ShittyTimer;

    private void Update()
    {
        if (!ShittyTimer.isPlaying)
        {
            ShittyTimer.Play(true);
        }

        transform.position = new Vector3(ShittyTimer.Value * 3.0f - 1.5f, transform.position.y, transform.position.z);
    }
}
