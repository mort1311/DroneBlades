using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ChargeTimer
{

    [SerializeField] private float timeToActive = 1.0f;

    public float timeElapsed;

    public bool IsCharged => timeElapsed > timeToActive;

    public ChargeTimer(float timeToActivate)
    {
        this.timeToActive = timeToActivate;
    }
    public void Reset()
    {
        timeElapsed = 0.0f;
    }

    public void Charge()
    {
        timeElapsed += Time.deltaTime;
    }

    void SetTimeToActivate(float time)
    {
        timeToActive = time;
    }
}
