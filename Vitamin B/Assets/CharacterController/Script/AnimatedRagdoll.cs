using UnityEngine;

public class AnimationModule : MonoBehaviour
{
    private Transform slave; // slave root (ragdoll)
    private Transform master; // master root (static animation)

    // ALL TRANSFORMS
    [SerializeField] private Transform[] slaveTransforms;
    [SerializeField] private Transform[] masterTransforms;

    // RIGIDBODIES
    private Transform[] slaveRigidTransforms;
    private Transform[] masterRigidTransforms;
    private Vector3[] rigidbodiesPosToCOM; // positions of rigidbodies relative to center of mass

    // JOINTS
    private ConfigurableJoint[] slaveConfigurableJoints;
    private Quaternion[] startLocalRotation;
    private Quaternion[] localToJointSpace;
    private JointDrive jointDrive = new JointDrive();

    private Vector3[] forceLastError;
    private int numOfRigids;
    [SerializeField] private bool isAlive = true;

    [SerializeField]
    private float
        forceCoefficient = 1.0f; // This is set by slaveController script in real time. Controls force applied to limbs.

    [SerializeField]
    private float
        torqueCoefficient =
            1.0f; // This is set by slaveController script in real time. Controls torque applied to limbs.

    // ALL ADJUSTABLE PARAMETERS
    [Range(0f, 340f)] [SerializeField] private float angularDrag = 0f; // Rigidbodies angular drag.
    [Range(0f, 2f)] [SerializeField] private float drag = 0.1f; // Rigidbodies drag.
    [Range(0f, 1000f)] [SerializeField] private float maxAngularVelocity = 1000f; // Rigidbodies maxAngularVelocity.
    [Range(0f, 10f)] [SerializeField] private float jointDamping = 0.6f;

    [Tooltip("Proportional force of PID controller.")] [Range(0f, 160f)]
    public float PForce = 8f;

    [Tooltip("Derivative force of PID controller.")] [Range(0f, .064f)]
    public float DForce = 0.01f;

    [Range(0f, 100f)] public float maxForce = 10f; // Limits the force
    [Range(0f, 10000f)] public float maxJointTorque = 2000f; // Limits the force

    public bool useGravity = true;

    
    private void Start()
    {
        int numOfRigids = 16;
        slaveRigidTransforms = new Transform[numOfRigids];
        masterRigidTransforms = new Transform[numOfRigids];
        rigidbodiesPosToCOM = new Vector3[numOfRigids];
        slaveConfigurableJoints = new ConfigurableJoint[numOfRigids];
        startLocalRotation = new Quaternion[numOfRigids];
        localToJointSpace = new Quaternion[numOfRigids];
        forceLastError = new Vector3[numOfRigids];


        int i = 0, j = 0;
        foreach (Transform t in slaveTransforms)
        {
            if (t.GetComponent<Rigidbody>() != null)
            {
                slaveRigidTransforms[i] = t;
                masterRigidTransforms[i] = masterTransforms[j];
                rigidbodiesPosToCOM[i] = Quaternion.Inverse(t.rotation) *
                                         (t.GetComponent<Rigidbody>().worldCenterOfMass - t.position);

                ConfigurableJoint cj = t.GetComponent<ConfigurableJoint>();
                if (cj != null) // ragdoll root (hips) doesn't have configurable joint
                {
                    slaveConfigurableJoints[i] = cj;

                    Vector3 forward = Vector3.Cross(cj.axis, cj.secondaryAxis);
                    Vector3 up = cj.secondaryAxis;

                    localToJointSpace[i] = Quaternion.LookRotation(forward, up);
                    startLocalRotation[i] = t.localRotation * localToJointSpace[i];

                    jointDrive = cj.slerpDrive;
                    cj.slerpDrive = jointDrive;
                }
                else if (i != 0) // if it's not root (hips)
                {
                    Debug.LogWarning("Rigidbody " + t + " doesn't have configurable joint" + "\n");
                    return;
                }

                i++;
            }

            j++;
        }

        foreach (Transform t in slaveRigidTransforms)
        {
            t.GetComponent<Rigidbody>().useGravity = useGravity;
            t.GetComponent<Rigidbody>().angularDrag = angularDrag;
            t.GetComponent<Rigidbody>().drag = drag;
            t.GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;
        }

        EnableJointLimits(true);
    }
    
    private void FixedUpdate()
    {
        FollowAnimation();
    }


    public void FollowAnimation()
    {
        SetJointTorque(maxJointTorque, jointDamping);

        for (int i = 0; i < slaveRigidTransforms.Length; i++) // Do for all rigidbodies of ragdoll
        {

            Rigidbody rb = slaveRigidTransforms[i].GetComponent<Rigidbody>();

            // Set rigidbody parameters in real-time
            rb.angularDrag = angularDrag;
            rb.drag = drag;
            rb.maxAngularVelocity = maxAngularVelocity;
            rb.useGravity = useGravity;

            // APPLY FORCE
            Vector3 masterRigidTransformsWCOM = masterRigidTransforms[i].position +
                                                masterRigidTransforms[i].rotation *
                                                rigidbodiesPosToCOM[i]; // WCOM = World Center Of Mass
            Vector3 forceError = masterRigidTransformsWCOM - rb.worldCenterOfMass;
            Vector3 forceSignal = PDControl(PForce, DForce, forceError, ref forceLastError[i]);
            forceSignal = Vector3.ClampMagnitude(forceSignal, maxForce * forceCoefficient);
            rb.AddForce(forceSignal, ForceMode.VelocityChange);

            // APPLY ROTATION
            if(i != 0 ) slaveConfigurableJoints[i].targetRotation = Quaternion.Inverse(localToJointSpace[i]) *
                                                        Quaternion.Inverse(masterRigidTransforms[i].localRotation) *
                                                        startLocalRotation[i];
        }
    }

    private Vector3 PDControl(float P, float D, Vector3 error, ref Vector3 lastError) // A PID controller
    {
        // This is the implemented algorithm:
        // signal = P * (error + D * derivative)
        Vector3 signal = P * (error + D * (error - lastError) / Time.fixedDeltaTime);
        lastError = error;
        return signal;
    }

    private void SetJointTorque(float positionSpring, float positionDamper)
    {
        for (int i = 1; i < slaveConfigurableJoints.Length; i++) // Do for all configurable joints
        {

            jointDrive.positionSpring = positionSpring  * torqueCoefficient;
            jointDrive.positionDamper = positionDamper;
            slaveConfigurableJoints[i].slerpDrive = jointDrive;
        }
    }

    private void EnableJointLimits(bool jointLimits)
    {
        for (int i = 1; i < slaveConfigurableJoints.Length; i++) // Do for all configurable joints
        {
            if (jointLimits)
            {
                slaveConfigurableJoints[i].angularXMotion = ConfigurableJointMotion.Limited;
                slaveConfigurableJoints[i].angularYMotion = ConfigurableJointMotion.Limited;
                slaveConfigurableJoints[i].angularZMotion = ConfigurableJointMotion.Limited;
            }
            else
            {
                slaveConfigurableJoints[i].angularXMotion = ConfigurableJointMotion.Free;
                slaveConfigurableJoints[i].angularYMotion = ConfigurableJointMotion.Free;
                slaveConfigurableJoints[i].angularZMotion = ConfigurableJointMotion.Free;
            }
        }
    }
}