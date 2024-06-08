using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnimationOnTrigger : MonoBehaviour
{
    public void PlayAnimation(string stateName)
    {
        GetComponent<Animator>().SetBool(stateName, true);
    }
}
