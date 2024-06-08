using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private float interactionradius;
    [SerializeField]
    private float CastOffset;

    [SerializeField]
    private GameObject SpeechBubblePrefab;
    [SerializeField] 
    private Vector3 SpeechBubbleOffset;
    
    public void interact()
    {
        RaycastHit hit;
        Physics.SphereCast(transform.position + transform.forward * CastOffset, interactionradius, transform.forward, out hit);
        if (hit.transform.gameObject.GetComponent<IGuest>() != null)
        {
            Instantiate(SpeechBubblePrefab, SpeechBubbleOffset, Quaternion.identity, hit.transform);
        }
    }
}
