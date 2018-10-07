using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BasicItemPickup : MonoBehaviour
{
    public int PlayerID = 1;
    public GameObject Character;
    public Camera CharacterCamera;
    public float MaximumItemDistance;

    private AudioSource m_AudioSource = null;
    private IPickable m_Pickable = null;

	void Update ()
	{
	    if (m_Pickable != null && Input.GetButtonDown("Player" + PlayerID.ToString() + "Interact"))
	    {
	        m_Pickable.OnPickup();
            PlayPickupSound(m_Pickable);
	        m_Pickable = null;
	    }
    }

    private void PlayPickupSound(IPickable pickable)
    {
        if (m_AudioSource == null)
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        AudioClip pickupClip = pickable.GetPickupClip();

        if (pickupClip != null)
        {
            m_AudioSource.clip = pickupClip;
            m_AudioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickable pickable = other.GetComponent<IPickable>();

        if (pickable != null)
        {
            m_Pickable = pickable;
        }
    }

    private void OnTriggerExit()
    {
        if (m_Pickable != null)
        {
            m_Pickable = null;
        }
    }
}
