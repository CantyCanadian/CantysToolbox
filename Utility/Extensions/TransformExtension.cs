using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEngine;

public static class TransformExtension
{
    /// <summary>
    /// Just like Transform.LookAt, but which rotates on a 2D axis, ignoring the Z axis.
    /// </summary>
    public static void LookAt2D(this Transform transform, Vector2 target)
    {
        Vector2 dir = target - transform.position.xy();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
