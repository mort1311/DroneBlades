using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ChargeTimer
{

    [SerializeField] private float timeToActive = 1.0f;

    private float timeElapsed;

    public bool IsCharged => timeElapsed > timeToActive;

    public void Reset()
    {
        timeElapsed = 0.0f;
    }

    public void Charge()
    {
        timeElapsed += Time.deltaTime;
    }
}
