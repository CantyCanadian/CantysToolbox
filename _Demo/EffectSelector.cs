using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Canty;

public class EffectSelector : MonoBehaviour
{
    public List<GameObject> ContextParents;
    public List<GameObject> ContextTexts;

    public Text ContextText;

    private int m_ContextIndex = 0;
    private bool m_ContextChanged = true;
    private bool m_ContentChanged = true;

    // Context 0
    public List<GameObject> PostProcessCameras;
    public List<GameObject> ShaderCubes;
    
    public Text PostProcessText;
    public Text ShaderText;

    private int m_CameraIndex = 0;
    private int m_CubesIndex = 0;

    private void ShaderContext()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MathUtil.DecrementWrap(ref m_CameraIndex, 1, 0, PostProcessCameras.Count);
            m_ContentChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MathUtil.IncrementWrap(ref m_CameraIndex, 1, 0, PostProcessCameras.Count);
            m_ContentChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            m_CameraIndex = 0;
            m_ContentChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            MathUtil.DecrementWrap(ref m_CubesIndex, 1, 0, ShaderCubes.Count);
            m_ContentChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            MathUtil.IncrementWrap(ref m_CubesIndex, 1, 0, ShaderCubes.Count);
            m_ContentChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            m_CubesIndex = 0;
            m_ContentChanged = true;
        }

        if (m_ContentChanged)
        {
            PostProcessCameras.SetAllActive(false);
            PostProcessCameras[m_CameraIndex].SetActive(true);
            PostProcessText.text = PostProcessCameras[m_CameraIndex].name;

            ShittyPlayer sp1 = PostProcessCameras[m_CameraIndex].GetComponent<ShittyPlayer>();
            if (sp1 != null)
            {
                sp1.Play();
            }

            ShaderCubes.SetAllActive(false);
            ShaderCubes[m_CubesIndex].SetActive(true);
            ShaderText.text = ShaderCubes[m_CubesIndex].name;

            ShittyPlayer sp2 = ShaderCubes[m_CubesIndex].GetComponent<ShittyPlayer>();
            if (sp2 != null)
            {
                sp2.Play();
            }

            m_ContentChanged = false;
        }
    }

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MathUtil.DecrementWrap(ref m_ContextIndex, 1, 0, ContextParents.Count);
            m_ContextChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            MathUtil.IncrementWrap(ref m_ContextIndex, 1, 0, ContextParents.Count);
            m_ContextChanged = true;
        }

        if (m_ContextChanged)
        {
            ContextParents.SetAllActive(false);
            ContextParents[m_ContextIndex].SetActive(true);

            ContextText.text = ContextParents[m_ContextIndex].name;

            m_CameraIndex = 0;
            m_CubesIndex = 0;

            m_ContentChanged = true;
            m_ContextChanged = false;
        }

        switch (m_ContextIndex)
        {
            case 0:
                ShaderContext();
                break;
        }
    }
}
