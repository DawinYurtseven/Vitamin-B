using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private float interactionradius;
    [SerializeField]
    private float raycastlength;
    
    private LayerMask mask;
    //[SerializeField]
    //private float CastOffset;

    [SerializeField]
    private GameObject SpeechBubblePrefab;
    [SerializeField] 
    private Vector3 SpeechBubbleOffset = new Vector3(-1.25f,2.25f,0f);
    
    public void interact()
    {
        RaycastHit hit;
        Physics.SphereCast(transform.position, interactionradius,transform.forward, out hit, raycastlength, ~4);
        if (hit.transform != null)
        {
            GameObject temp = Instantiate(SpeechBubblePrefab, SpeechBubbleOffset, Quaternion.identity, hit.transform);
            temp.GetComponent<SpeechBubbleController>().target = hit.transform.GetComponent<IGuest>();
        }
    }

    private void Awake()
    {
        StartCoroutine(delayedActivation());
    }

    IEnumerator delayedActivation()
    {
        yield return new WaitForSeconds(1.0f);
        interact();
    }
    
}
