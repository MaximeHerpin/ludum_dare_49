using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PID
{
    public float correctionMultiplier = 1;
    public float kp, ki, kd;
    private float error, integratedError, derivatedError;
    private float previousError;

    public PID(float p, float i, float d)
    {
        kp = p;
        ki = i;
        kd = d;
    }

    public float GetCorrection(float currentError, float deltaTime)
    {
        if (deltaTime == 0)
        {
            return 0;
        }
        error = currentError;
        integratedError += error * deltaTime;
        derivatedError = (error - previousError) / deltaTime;
        previousError = currentError;

        return (error * kp + integratedError * ki + derivatedError * kd) * correctionMultiplier;
    }
}