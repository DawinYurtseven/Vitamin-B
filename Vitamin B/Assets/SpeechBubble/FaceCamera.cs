using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FaceCamera : MonoBehaviour
{
    private Transform CamPos;

    private void Awake()
    {
        CamPos = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(CamPos);
    }
}
