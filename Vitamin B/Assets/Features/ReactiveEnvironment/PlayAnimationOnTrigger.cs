using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnTrigger : MonoBehaviour
{

    public Animator animator;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string stateName)
    {
        if (animator != null)
            animator.SetBool(stateName, true);
    }
    
    public void SetTrigger(string trigger)
    {
        if (animator != null)
            animator.SetTrigger(trigger);
    }

    private void LateUpdate()
    {

    }
}
