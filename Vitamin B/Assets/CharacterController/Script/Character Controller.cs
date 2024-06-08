using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    #region Values

    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject followTarget;
    [SerializeField] private Camera cam;
    [SerializeField] private float uprightTorque;
    [SerializeField] private AnimationCurve uprightTorqueCurve, directionalAngleCurve;
    [SerializeField] private float cameraXSpeed, cameraYSpeed;

    #endregion

    #region HelperValues

    private Rigidbody rb;
    private Vector2 movementVector;
    private float currentSpeed;

    private bool lockRotate = true;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Balance();
    }


    #region Input

    public void MoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lockRotate = false;
            
            movementVector = context.ReadValue<Vector2>();
            currentSpeed = Mathf.Clamp(currentSpeed + Time.deltaTime * 5, 0, speed);
        }
        else if (context.canceled)
        {
            movementVector = Vector2.zero;
            lockRotate = true;
        }
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
        }
    }

    public void CancelInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
        }
    }


    public void CameraControlInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 delta = context.ReadValue<Vector2>().normalized;
            Debug.Log(delta);

            followTarget.transform.Rotate(Vector3.up, delta.x * cameraXSpeed * Time.deltaTime);
            followTarget.transform.Rotate(Vector3.right, delta.y * cameraYSpeed * Time.deltaTime);
            
            followTarget.transform.localEulerAngles = new Vector3(followTarget.transform.localEulerAngles.x, followTarget.transform.localEulerAngles.y, 0);
            var angles = followTarget.transform.localEulerAngles;
            var angle = followTarget.transform.localEulerAngles.x;
            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            followTarget.transform.localEulerAngles = angles;
        }
    }

    #endregion

    #region Physics

    void Move()
    {
        if (movementVector.magnitude == 0)
        {
            currentSpeed = Mathf.Clamp(currentSpeed - Time.deltaTime * 5, 0, speed);
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            currentSpeed = Mathf.Clamp(currentSpeed + Time.deltaTime * 5, 0, speed);
            
            var angles = followTarget.transform.localEulerAngles;
            transform.Rotate(Vector3.up, angles.y);
            followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            
        }

        Vector3 moveDirection = new Vector3(movementVector.x, 0, movementVector.y);
        Vector3 move = transform.TransformDirection(moveDirection);
        rb.velocity = Vector3.Lerp(rb.velocity, move * currentSpeed, Time.deltaTime * 5);
    }

    void Balance()
    {
        var balancePer = Vector3.Angle(transform.up, Vector3.up) / 180;
        balancePer = uprightTorqueCurve.Evaluate(balancePer);
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up).normalized;

        rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightTorque * balancePer);


        
    }

    #endregion
}