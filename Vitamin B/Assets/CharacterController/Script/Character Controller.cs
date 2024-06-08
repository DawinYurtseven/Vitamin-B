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
    [SerializeField] private GameObject lookTarget, followTarget;
    [SerializeField] private float uprightTorque;
    [SerializeField] private AnimationCurve uprightTorqueCurve, directionalAngleCurve;
    [SerializeField] private float cameraXSpeed, cameraYSpeed;

    #endregion

    #region HelperValues

    private Rigidbody rb;
    private Vector2 movementVector;
    private float currentSpeed;

    private bool lockRotate = false;

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
            var angles = followTarget.transform.localEulerAngles;
            transform.rotation = Quaternion.Euler(0, angles.y, 0);
            followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

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
            Vector2 delta = context.ReadValue<Vector2>();

            followTarget.transform.Rotate(Vector3.up, delta.x * cameraXSpeed * Time.deltaTime);
            followTarget.transform.Rotate(Vector3.right, delta.y * cameraYSpeed * Time.deltaTime);
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
        }
        else
        {
            currentSpeed = Mathf.Clamp(currentSpeed + Time.deltaTime * 5, 0, speed);
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


        var directionAnglePer = 0.0f;

        if (!lockRotate)
        {
            directionAnglePer = Vector3.SignedAngle(transform.forward, lookTarget.transform.forward, Vector3.up) / 180;
        }

        if (Mathf.Abs(directionAnglePer * 25) < 0.25f)
        {
            directionAnglePer = 0.0f;
            rb.angularVelocity = Vector3.zero;
        }

        rb.AddRelativeTorque(0, directionAnglePer * 25, 0);
    }

    #endregion
}