using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleDashEnemy : Enemy
{
    [SerializeField] float attackForce;
    [SerializeField] int dashCount = 3;
    [SerializeField] float timeBeforeDash = 0.6f;
    [SerializeField] float timeBeforAttack = 3;
    float attackForceStart;
    [SerializeField] float attackForceAcceleration = 2;
    [SerializeField] float attackForceAccelerationStart;
    [SerializeField] float velocityDrag = 0.98f;
    void Start()
    {
        setAttackDamage(1);
        setHealth(3);
        setTarget(GameObject.FindGameObjectWithTag("Player"));
        attackForceStart = attackForce;
        attackForceAccelerationStart = attackForceAcceleration;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = rigidbody.velocity * velocityDrag;
        WarningCooldownTimerHandler();
        //StartTimer("timer");
        if (timer > timeBeforAttack)
        {
            Attack();
           // StopTimer("timer");
        }
        
    }

    override protected void Attack()
    {

        StartCoroutine(ThirdHitTargetDashAttack());
    }

    IEnumerator RandomDashAttack()
    {
        int random = Random.Range(0, 2);
        int i = 0;
        while (i < dashCount)
        {
            rigidbody.velocity = Vector2.zero;
            if (random == 1)
            {
                rigidbody.AddForce((target.transform.position - transform.position).normalized * attackForce, ForceMode2D.Impulse);
            }
            else
            {
                rigidbody.AddForce(new Vector2(Random.value, Random.value).normalized * attackForce, ForceMode2D.Impulse);
            }
            i++;
            while (random != Random.Range(0, 2))
            {
                random = Random.Range(0, 2);
            }
            yield return new WaitForSeconds(timeBeforeDash);
        }
    }

    IEnumerator ThirdHitTargetDashAttack()
    {
        int i = 0;
        while ( i < dashCount)
        {
            attackForce += attackForceAcceleration;
            rigidbody.velocity = Vector2.zero;
            if (i == 2)
            {

                rigidbody.AddForce((target.transform.position - transform.position).normalized * attackForce, ForceMode2D.Impulse);
            }
            else
            {
                rigidbody.AddForce(new Vector2(Random.value, Random.value).normalized * attackForce, ForceMode2D.Impulse);
            }
            attackForceAcceleration += attackForceAcceleration/2;
            i++;
            yield return new WaitForSeconds(timeBeforeDash);
        }
        attackForceAcceleration = attackForceAccelerationStart;
        attackForce = attackForceStart;
    }
}
