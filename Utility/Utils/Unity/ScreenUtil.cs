using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Canty
{
    public static class ScreenUtil
    {
        private static Camera s_MainCamera = null;

        public static void MouseToScreenPosition(ref RaycastHit hit, LayerMask layerToHit, float percentScaling = 1.0f)
        {
            if (s_MainCamera == null)
            {
                s_MainCamera = Camera.main;
            }

            Vector2 mousePos = Input.mousePosition;
#if UNITY_EDITOR
            Vector2 gameView = Handles.GetMainGameViewSize();
#else
            Vector2 gameView = new Vector2(Screen.width, Screen.height);
#endif
            mousePos.x = Mathf.Clamp(mousePos.x, gameView.x * (1.0f - percentScaling), gameView.x - gameView.x * (1.0f - percentScaling));
            mousePos.y = Mathf.Clamp(mousePos.y, gameView.y * (1.0f - percentScaling), gameView.y - gameView.y * (1.0f - percentScaling));

            Ray ray = s_MainCamera.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0.0f));
            Physics.Raycast(ray, out hit, layerToHit);
        }
    }
}