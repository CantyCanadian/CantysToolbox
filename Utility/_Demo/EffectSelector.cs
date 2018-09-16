using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSelector : MonoBehaviour
{
    public List<Camera> PostProcessCameras;
    public List<GameObject> ShaderCubes;

    public Text PostProcessText;
    public Text ShaderText;

    private int m_CameraIndex = 0;
    private int m_CubesIndex = 0;

    private bool m_Changed = true;

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Q))
        {
            MathUtil.DecrementWrap(ref m_CameraIndex, 1, 0, PostProcessCameras.Count);
            m_Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            MathUtil.IncrementWrap(ref m_CameraIndex, 1, 0, PostProcessCameras.Count);
            m_Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_CameraIndex = 0;
            m_Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MathUtil.DecrementWrap(ref m_CubesIndex, 1, 0, ShaderCubes.Count);
            m_Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MathUtil.IncrementWrap(ref m_CubesIndex, 1, 0, ShaderCubes.Count);
            m_Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            m_CubesIndex = 0;
            m_Changed = true;
        }

        if (m_Changed)
        {
            PostProcessCameras.SetAllActive(false);
            PostProcessCameras[m_CameraIndex].gameObject.SetActive(true);
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

            m_Changed = false;
        }
    }
}
