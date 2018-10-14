///====================================================================================================
///
///     PostProcessOutlineShader by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty.Shaders
{
    /// <summary>
    /// Class to use when using outline shader.
    /// </summary>
    public class PostProcessOutlineShader : MonoBehaviour
    {
        public Material ShaderMaterial;
        public LayerMask OutlineMask;

        private Camera m_MainCamera;
        private Camera m_RenderCamera;
        private RenderTexture m_RenderTexture;
        private RenderTexture m_OppositeTexture;

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            Graphics.Blit(src, dst);

            bool seeThrough = ShaderMaterial.GetInt("_SeeThrough") == 1;

            if (!seeThrough)
            {
                m_RenderCamera.targetTexture = m_OppositeTexture;
                m_RenderCamera.cullingMask = ~OutlineMask;
                m_RenderCamera.Render();

                ShaderMaterial.SetTexture("_OppositeTex", m_OppositeTexture);
            }

            ShaderMaterial.SetInt("_SeeThrough", seeThrough ? 1 : 0);
            ShaderMaterial.SetTexture("_RendererTex", m_RenderTexture);
            Graphics.Blit(m_RenderTexture, src, ShaderMaterial, 0);

            ShaderMaterial.SetTexture("_OutlineTex", src);
            Graphics.Blit(src, dst, ShaderMaterial, 1);
        }

        private void Start()
        {
            m_MainCamera = GetComponent<Camera>();

            if (m_MainCamera == null)
            {
                m_MainCamera = Camera.main;
            }

            GameObject outlineGameObject = new GameObject("OutlineCamera");
            outlineGameObject.transform.SetParent(gameObject.transform);

            m_RenderTexture = new RenderTexture(m_MainCamera.pixelWidth, m_MainCamera.pixelHeight, 16, RenderTextureFormat.R8);
            m_OppositeTexture = new RenderTexture(m_MainCamera.pixelWidth, m_MainCamera.pixelHeight, 16, RenderTextureFormat.R8);

            m_RenderCamera = outlineGameObject.AddComponent<Camera>();
            m_RenderCamera.CopyFrom(m_MainCamera);
            m_RenderCamera.backgroundColor = Color.black;
            m_RenderCamera.clearFlags = CameraClearFlags.SolidColor;
            m_RenderCamera.rect = new Rect(0, 0, 1, 1);
            m_RenderCamera.cullingMask = OutlineMask;
            m_RenderCamera.targetTexture = m_RenderTexture;
            m_RenderCamera.allowHDR = false;
        }

        private void OnDestroy()
        {
            if (m_RenderTexture != null)
            {
                m_RenderTexture.Release();
            }

            if (m_OppositeTexture != null)
            {
                m_OppositeTexture.Release();
            }
        }
    }
}