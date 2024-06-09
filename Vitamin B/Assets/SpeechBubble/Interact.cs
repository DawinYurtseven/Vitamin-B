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
        //Physics.Raycast(transform.position + new Vector3(0,1,0),transform.forward, out hit, raycastlength, ~4, QueryTriggerInteraction.Collide);
        Physics.SphereCast(transform.position + new Vector3(0,1,0), interactionradius,transform.forward, out hit, raycastlength, ~4, QueryTriggerInteraction.Collide);
        //Debug.Log(hit.transform);
        if (hit.transform != null)
        {
            if (hit.transform.GetComponent<IGuest>() != null)
            {
                hit.transform.GetComponent<IGuest>().Interact();
            }
            else
            {
                Debug.Log("test");
            }
                
        }
        
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0,1,0), transform.forward * raycastlength);
    }

    /*private void Awake()
    {
        StartCoroutine(delayedActivation());
    }*/

    IEnumerator delayedActivation()
    {
        yield return new WaitForSeconds(1.0f);
        interact();
    }
    
}
