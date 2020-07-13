using UnityEditor;
using UnityEngine;

namespace Canty.Editors
{
    public class CameraMenuItems
    {
        /// <summary>
        /// Dual camera system made to render the player character above the world in a first person setting.
        /// </summary>
        [MenuItem("GameObject/Custom Cameras/First-Person Camera", false, 0)]
        public static void CreateFirstPersonCamera(MenuCommand command)
        {
            GameObject obj = EditorUtil.CreateGameObjectInWorld(command, "First Person Camera");

            obj.AddComponent<Camera>();

            GameObject localCamera = new GameObject("LocalCamera");

            localCamera.transform.SetParent(obj.transform);
            localCamera.transform.position = obj.transform.position;
            localCamera.transform.rotation = obj.transform.rotation;

            Camera camera = localCamera.AddComponent<Camera>();

            camera.clearFlags = CameraClearFlags.Depth;
            camera.cullingMask = 0;

            Debug.Log("First-Person Camera : To finish creating the camera, set [First Person Camera]'s Culling Mask to remove your player layer, then set the [Local Camera]'s Culling Mask to only render the player layer.");
            EditorUtility.DisplayDialog("Reminder", "To finish creating the camera, set [First Person Camera]'s Culling Mask to remove your player layer, then set the [Local Camera]'s Culling Mask to only render the player layer.", "OK");
        }
    }
}