using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public static class ScreenUtil
    {
        private static Camera m_MainCameraCache = null;

        public static void MouseToScreenPosition(ref RaycastHit hit)
        {
            if (m_MainCameraCache == null)
            {
                m_MainCameraCache = Camera.main;
            }

            Ray ray = m_MainCameraCache.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Physics.Raycast(ray, out hit, 10.0f, LayerMask.GetMask("MouseRaycast"));
        }
    }
}