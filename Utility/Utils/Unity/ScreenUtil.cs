using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public static class ScreenUtil
    {
        public static void MouseToScreenPosition(ref RaycastHit hit)
        {
            Ray ray = CameraManager.Instance.GetCamera().ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Physics.Raycast(ray, out hit, 10.0f, LayerMask.GetMask("MouseRaycast"));
        }
    }
}