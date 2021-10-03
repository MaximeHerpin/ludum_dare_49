using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Vector3 centerOfMass;
    public List<WheelCollider> wheels;
    public float speed = 1;
    public float midAirStabilisation;
    public float maxTorqueCorrection = 100;
    public AudioSource motorSound;
    public PID torqueController;
    private Rigidbody rb;
    [SerializeField]private float soundPitch = 1;

    public float targetSpeed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.centerOfMass = centerOfMass;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + centerOfMass, .1f);
    }

    void FixedUpdate()
    {
        soundPitch = Mathf.Lerp(soundPitch, 1+Mathf.Max(0, wheels[0].rpm) / 700, Time.deltaTime * 4);
        motorSound.volume = .5f + Mathf.Max(0, wheels[0].rpm) / 700;
        motorSound.pitch = soundPitch;

        UpdateTorque();
        if (!IsGrounded())
        {
            StayStableInMidAir();
        }
    }

    public void UpdateTorque()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        targetSpeed = horizontalInput * speed;


        foreach (var wheel in wheels)
        {
            float speedDelta = targetSpeed - wheel.rpm;
            float maxCorrectionMagnitude = maxTorqueCorrection * Time.deltaTime;
            float correction = Mathf.Clamp(torqueController.GetCorrection(speedDelta, Time.deltaTime), -maxCorrectionMagnitude, maxCorrectionMagnitude);
            wheel.motorTorque += correction;
        }
    }

    public void StayStableInMidAir()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * midAirStabilisation);
    }

    bool IsGrounded()
    {
        bool grounded = false;
        foreach (var wheel in wheels)
        {
            if (wheel.isGrounded)
            {
                grounded = true;
                break;
            }
        }
        return grounded;
    }
}
