using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyAirForce : MonoBehaviour
{
    Rigidbody2D rigidbody;
    //mouseposition
    SpriteRenderer spriteRenderer;
    [SerializeField] float dashforce = 5;
    [SerializeField] float chargedDashForce = 10;
    [SerializeField] float tempDashForce = 5;
    Vector3 clickedPosition;
    Vector3 dashDirection;
    [SerializeField] float velocityDrag = 0.98f;

    [SerializeField] float timeToActivate = 1;
    float timer = 0;
    [SerializeField] float activationDuration = 1;
    float activationTimer = 0;

    [SerializeField] float timerCollisionForce;
    [SerializeField] float timerCollisionForceEnd;
    bool startTimerCollisionForce = false;

    [SerializeField] Vector2 swingVector;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();

        swingVector = new Vector2(1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        ActivationHandler();
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddForce(new Vector2(1,0).normalized*20, ForceMode2D.Force);
           // swingVector = new Vector2(swingVector.x + 0.01f, swingVector.y - 0.01f);
        }
        
    }

    void ActivationHandler()
    {
        if (Input.GetMouseButton(0))
        {
            timer = timer + Time.deltaTime;
        }
        if (timer > timeToActivate)
        {
            activationTimer = activationTimer + Time.deltaTime;
            spriteRenderer.color = Color.red;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (timer > timeToActivate)
            {

                tempDashForce = chargedDashForce;
            }
            else
            {
                tempDashForce = dashforce;
            }

            clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashDirection = (clickedPosition - transform.position).normalized;
            rigidbody.AddForce(dashDirection * tempDashForce, ForceMode2D.Impulse);



        }
        if (activationTimer > activationDuration)
        {
            timer = 0;
            activationTimer = 0;
            spriteRenderer.color = Color.white;
        }
        rigidbody.velocity = rigidbody.velocity * velocityDrag;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.otherRigidbody.velocity = Vector2.zero;
        collision.otherRigidbody.AddForce((transform.position - collision.gameObject.transform.position).normalized * 40, ForceMode2D.Impulse);
    }
}
