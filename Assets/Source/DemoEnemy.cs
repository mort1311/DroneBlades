using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEnemy : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float rangeRadius = 3;
    [SerializeField] bool isTargetInRange = false;

    Rigidbody2D rigidbody;

    //-----------------------------------------TIMERS--------------------------------------------
    
    [SerializeField] float attackPhaseDuration = 5;
    ChargeTimer attackPhaseTimer;
    bool shouldStartAttackPhaseTimer = false;
    [SerializeField] float warningDuration = 1f;

    bool shouldActivateWarning = false;

    //-----------------------------------------ATTACK-------------------------------------------

    [SerializeField] float dashForce = 30;
    [SerializeField] bool shouldAttack = false;
    
    //==========================================START=============================================


    void Start()
    {
        attackPhaseTimer = new ChargeTimer(attackPhaseDuration);
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //ListenForTarget();
        ListenForTargetInRange();
        ListenForAttackPhaseEnd();

        Debug.Log(attackPhaseTimer.timeElapsed);
    }

    private void FixedUpdate()
    {
        ListenForShouldAttack();
    }

    //-----------------------------------------ATTACK-------------------------------------------


    void ListenForTargetInRange()
    {
        // Checks if target is in range
        if (Vector2.Distance(target.transform.position, transform.position) < rangeRadius)
        {
            isTargetInRange = true;
        }
        else isTargetInRange = false;

        // Activates the attackPhaseTimer and Warning
        if (isTargetInRange && attackPhaseTimer.timeElapsed == 0)
        {
            shouldStartAttackPhaseTimer = true;
            shouldActivateWarning = true;
        }

        if (shouldStartAttackPhaseTimer)
        {
            attackPhaseTimer.Charge();
            ActivateWarning();
        }
    }

    void Attack()
    {
        rigidbody.AddForce((target.transform.position - transform.position).normalized * dashForce, ForceMode2D.Impulse);
        shouldAttack = false;
    }

    void ListenForShouldAttack()
    {
        if (shouldAttack == true)
        {
            Attack();
        }
    }

    void ListenForAttackPhaseEnd()
    {
        if (attackPhaseTimer.timeElapsed > attackPhaseDuration)
        {
            attackPhaseTimer.Reset();
            shouldStartAttackPhaseTimer = false;
        }
    }

    void ActivateWarning()
    {
        //Animator.SetTrigger("WarningTrigger")
        if (attackPhaseTimer.timeElapsed > warningDuration && shouldActivateWarning)
        {
            shouldAttack = true;
            shouldActivateWarning = false;
        }
    }

    //---------------------------------------IENUMERATORS---------------------------------------

    IEnumerator C_WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }


 

  

 


   
}
