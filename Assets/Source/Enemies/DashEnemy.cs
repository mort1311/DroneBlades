using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : Enemy
{
    [SerializeField] float attackForce;
    void Start()
    {
        setAttackDamage(1);
    
        setTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        WarningCooldownTimerHandler();
        AttackWarning();
    }

    override protected void Attack()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce((target.transform.position-transform.position).normalized*attackForce,ForceMode2D.Impulse);
    }


}
