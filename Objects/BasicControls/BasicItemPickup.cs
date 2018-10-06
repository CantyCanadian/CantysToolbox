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

	void Update ()
	{
	    RaycastHit hit;
	    if (Physics.Raycast(CharacterCamera.transform.position, CharacterCamera.transform.forward, out hit))
	    {
	        if (Vector3.Distance(hit.transform.position, Character.transform.position) <= MaximumItemDistance)
	        {
	            IPickable pickable = hit.transform.GetComponent<IPickable>();

	            if (pickable != null)
	            {
                    Debug.Log("Seeing that item");
	                if (Input.GetButtonDown("Player" + PlayerID.ToString() + "Interact"))
	                {
                        pickable.OnPickup();
	                }
	            }
            }
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
}
