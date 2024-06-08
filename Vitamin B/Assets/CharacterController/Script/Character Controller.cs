using UnityEngine;
using UnityEngine.InputSystem;


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

    private Rigidbody _rb;
    private Vector2 _movementVector;
    private float _currentSpeed;

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
            _movementVector = context.ReadValue<Vector2>();
            _currentSpeed = Mathf.Clamp(_currentSpeed + Time.deltaTime * 5, 0, speed);
        }
        else if (context.canceled)
        {
            _movementVector = Vector2.zero;
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
            Debug.Log(delta.x * cameraXSpeed * Time.deltaTime);

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
            _rb.angularVelocity = Vector3.zero;
        }
        else
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed + Time.deltaTime * 5, 0, speed);
            var angles = followTarget.transform.localEulerAngles;
            transform.Rotate(Vector3.up, angles.y);
            followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        }

        Vector3 moveDirection = new Vector3(_movementVector.x, 0, _movementVector.y);
        Vector3 move = transform.TransformDirection(moveDirection);
        _rb.velocity = Vector3.Lerp(_rb.velocity, move * _currentSpeed, Time.deltaTime * 5);
    }

    void Balance()
    {
        var balancePer = Vector3.Angle(transform.up, Vector3.up) / 180;
        balancePer = uprightTorqueCurve.Evaluate(balancePer);
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up).normalized;

        _rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * (uprightTorque * balancePer));

        var angle = (followTarget.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y) / 180;

        if (angle > 0.5)
        {
            Debug.Log($"{angle} with {followTarget.transform.rotation.eulerAngles.y} and {transform.rotation.eulerAngles.y}");
            var percent = directionalAngleCurve.Evaluate(angle);
            if (angle < 0) percent *= -1;
            _rb.AddRelativeTorque(0, percent * 500, 0);
        }

        
    }
}

#endregion

