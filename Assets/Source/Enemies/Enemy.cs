using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Main Props
    [SerializeField] protected int attackDamage;
    [SerializeField] protected int health;
    [SerializeField] protected GameObject target;
    [SerializeField] protected Rigidbody2D rigidbody;
    //Timers
    [SerializeField] private ChargeTimer warningChargeTimer;

    [SerializeField] protected float timer;
    [SerializeField] protected float timerTriggerPoint;
    [SerializeField] protected float timer2;
    [SerializeField] protected float timer2TriggerPoint;
    [SerializeField] protected float timer3;
    [SerializeField] protected float timer3TriggerPoint;

    [SerializeField] protected float warningRadius;
    [SerializeField] protected float warningDistance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual protected void Attack()
    {

    }

    protected void WarningCooldownTimerHandler()
    {
        if (warningTimerStarted)
        {
            warningChargeTimer.Charge();
            if (warningChargeTimer.IsCharged)
            {
                warningChargeTimer.Reset();            
            }
        }
    }

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float timerAttackCooldown;
    [SerializeField] protected float timerAttackCooldownEnd;
    [SerializeField] protected bool warningTimerStarted = false;
    protected void AttackWarning()
    {
        if (timerAttackCooldown == 0)
        {
            if (Vector2.Distance(target.transform.position, transform.position) < warningDistance)
            {
                warningTimerStarted = true;
                Attack();
            }
        }
    }

    //Timer
    protected void StartTimerWarningTimer() 
    {
        warningChargeTimer.Charge();
    }


    public GameObject getTarget()
    {
        return target;
    }
    public void setTarget(GameObject newTarget)
    {
        target = newTarget;
    }
    public int getAttackDamage()
    {
        return attackDamage;
    }
    public int getHealth()
    {
        return health;
    }

    public void setAttackDamage(int newAttackDamage)
    {
        attackDamage = newAttackDamage;
    }
    public void setHealth(int newHealth)
    {
        health = newHealth;
    }
}
