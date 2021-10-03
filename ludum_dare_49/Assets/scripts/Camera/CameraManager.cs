using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Transform target;
    public Vector3 offset;
    public PID controller;
    public float levelViewOffset = 10;
    public float distance;
    public float targetDistance;
    public float distanceSmoothness;
    private Bounds focusBounds;
    private PID[] axisControllers;
    public PostProcessProfile postProcessing;

    public Vector3 smoothedTarget;

    private void Awake()
    {
        instance = this;
    }


    public void SetFocusBounds(Bounds bounds)
    {
        focusBounds = bounds;
    }

    void Start()
    {
        smoothedTarget = target.position + offset;
        targetDistance = distance;
        axisControllers = new PID[]{new PID(0,0,0), new PID(0,0,0), new PID(0,0,0) };
    }

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * distanceSmoothness);
        DepthOfField dph;
        if (postProcessing.TryGetSettings<DepthOfField>(out dph))
        {
            dph.focusDistance.value = distance;
        }
        Vector3 targetPosition = target.position + offset;
        targetPosition.x = Mathf.Max(targetPosition.x, focusBounds.min.x + levelViewOffset * distance);
        Vector3 error = targetPosition - smoothedTarget;
        Vector3 correction = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            axisControllers[i].kd = controller.kd;
            axisControllers[i].ki = controller.ki;
            axisControllers[i].kp = controller.kp;
            axisControllers[i].correctionMultiplier = controller.correctionMultiplier;

            correction[i] = axisControllers[i].GetCorrection(error[i], Time.deltaTime);
        }

        //smoothedTarget = Vector3.Lerp(smoothedTarget, target.position + offset, Time.deltaTime * smoothness);
        smoothedTarget += correction;
        transform.position = smoothedTarget - transform.forward * distance;
    }


}
