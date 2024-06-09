using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterController : MonoBehaviour
{
    #region Values

    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject followTarget;
    [SerializeField] private Camera cam;
    [SerializeField] private float height;
    [SerializeField] private float cameraXSpeed, cameraYSpeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject master, slave;

    [SerializeField] private GameObject hip;
    
    #endregion

    #region HelperValues
    
    private Vector2 _movementVector;
    private float _currentSpeed;

    #endregion

    private void Update()
    {
        Move();
        //Balance();
        CheckHeight();
    }


    #region Input

    public void MoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementVector = context.ReadValue<Vector2>();
            _currentSpeed = Mathf.Clamp(_currentSpeed + Time.deltaTime * 5, 0, speed);
            _animator.SetBool("Walking", true);
        }
        else if (context.canceled)
        {
            _movementVector = Vector2.zero;
            _animator.SetBool("Walking", false);
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

            followTarget.transform.Rotate(Vector3.up, delta.x * cameraXSpeed * Time.deltaTime);
            followTarget.transform.Rotate(Vector3.right, delta.y * cameraYSpeed * Time.deltaTime);

            followTarget.transform.localEulerAngles = new Vector3(followTarget.transform.localEulerAngles.x,
                followTarget.transform.localEulerAngles.y, 0);
            var angles = followTarget.transform.localEulerAngles;
            var angle = followTarget.transform.localEulerAngles.x;
            if (angle is > 180 and < 340)
            {
                angles.x = 340;
            }
            else if (angle is < 180 and > 40)
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
        if (_movementVector.magnitude == 0)
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed - Time.deltaTime * 5, 0, speed);
        }
        else
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed + Time.deltaTime * 5, 0, speed);
            var angles = followTarget.transform.localEulerAngles;
            transform.Rotate(Vector3.up, angles.y);
            followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        }

        Vector3 moveDirection = new Vector3(_movementVector.x, 0, _movementVector.y);
        Vector3 move = transform.TransformDirection(moveDirection) + CalculatePushBackMovement();

        transform.position =
            Vector3.Lerp(transform.position, transform.position + move * _currentSpeed, Time.deltaTime);
        
    }

    void CheckHeight()
    {
        var hit = new RaycastHit();
        Debug.DrawRay(hip.transform.position, Vector3.down * height, Color.red);
        bool isHit = Physics.Raycast(hip.transform.position, hip.transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity,LayerMask.GetMask("Ground"));
        if (isHit) {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    
    
    
    private Vector3 CalculatePushBackMovement()
    {
        Vector3 slaveToMasterVector = (slave.transform.position - master.transform.position);
        float distance = slaveToMasterVector.magnitude;

        //keep in mind
        float ratio = Mathf.Clamp(distance / 0.5f, 0, 1);
        float magnitude = Mathf.SmoothStep(0, 1, ratio);
        Vector3 moveVector = Vector3.ClampMagnitude(slaveToMasterVector, magnitude);

        moveVector.y = 0;
        return moveVector;
    }

}

#endregion

