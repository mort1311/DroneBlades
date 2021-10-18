using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{
    [SerializeField] float followForce = 2;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        FollowTarget(target);
    }

    void FollowTarget(GameObject target)
    {
        rigidbody.AddForce((target.transform.position - transform.position).normalized * followForce, ForceMode2D.Force); 
    }
}
