using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed = 10;
    void Start()
    {
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
