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

   
    
    public void interact()
    {
        RaycastHit hit;
        Physics.SphereCast(transform.position, interactionradius,transform.forward, out hit, raycastlength, ~4);
        if (hit.transform != null)
        {
            hit.transform.GetComponent<IGuest>().Interact();
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
