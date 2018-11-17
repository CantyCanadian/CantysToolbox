using Canty;
using UnityEditor;
using UnityEngine;

namespace Canty.Editors
{
    public class CameraMenuItems
    {
        /// <summary>
        /// Replica of the default camera context menu option in order to place it in a Camera submenu in the hierarchy.
        /// </summary>
        [MenuItem("GameObject/Camera/Camera", false, 10)]
        public static void CreateDefaultCamera(MenuCommand command)
        {
            GameObject obj = EditorUtil.CreateMenuItemGameObject(command, "Camera");

            obj.AddComponent<Camera>();
            obj.AddComponent<FlareLayer>();
            obj.AddComponent<AudioListener>();
        }

        /// <summary>
        /// Dual camera system made to render the player character above the world in a first person setting.
        /// </summary>
        [MenuItem("GameObject/Camera/First-Person Camera", false, 10)]
        public static void CreateFirstPersonCamera(MenuCommand command)
        {
            GameObject obj = EditorUtil.CreateMenuItemGameObject(command, "FirstPersonCamera");

            obj.AddComponent<Camera>();

            GameObject localCamera = new GameObject("LocalCamera");

            localCamera.transform.SetParent(obj.transform);

            Camera camera = localCamera.AddComponent<Camera>();

            camera.clearFlags = CameraClearFlags.Depth;
            camera.cullingMask = 0;

            Debug.Log("First-Person Camera : To finish creating camera, set Parent Camera's Culling Mask to remove your player, while setting the Child's Culling Mask to only render the player.");
        }
    }
}