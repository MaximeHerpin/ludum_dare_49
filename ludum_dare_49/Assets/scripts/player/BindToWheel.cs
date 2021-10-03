using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindToWheel : MonoBehaviour
{
    private WheelCollider wheelCollider;
    private void Start()
    {
        wheelCollider = GetComponentInParent<WheelCollider>();
    }

    private void Update()
    {
        wheelCollider.GetWorldPose(out var pos, out var rot);
        transform.position = pos;
        transform.rotation = rot;
    }
}
