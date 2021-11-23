using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    //--------------------------------JOYSTICKS--------------------------------

    [SerializeField] FloatingJoystick joystickRight;
    [SerializeField] FloatingJoystick joystickLeft;
    Vector2 joystickRightDirection;
    Vector2 joystickLeftDirection;
    bool isJoystickRightHeld;
    bool isJoystickLeftHeld;

    //--------------------------------RIGIDBODY---------------------------------                               

    Rigidbody2D rigidbody;

    //--------------------------------TIMERS------------------------------------

    [SerializeField] private ChargeTimer rightChargeTimer;
    [SerializeField] private ChargeTimer leftChargeTimer;

    //--------------------------------MOVEMENT----------------------------------

    [SerializeField] float dashForce = 20;    
    [SerializeField] float moveForce = 1;
    bool shouldDash = false;
    bool shouldMove = false;


    //===============================START===============================================================================================
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ListenForJoystickLeftInput();
        ListenForJoystickRightInput();
    }

    private void FixedUpdate()
    {
        ListenForDash();
        ListenForMove();
    }

    
    // ---------------------------------MOVEMENT------------------------------------------

                        //  Dash
                        //  Move
    void Dash()                                                 // rigidbody, AddForce
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(joystickRightDirection * dashForce, ForceMode2D.Impulse);
    }

    void Move()                                                 // rigidbody, AddForce
    {
        rigidbody.AddForce(joystickLeftDirection.normalized * moveForce, ForceMode2D.Force);
    }


    // --------------------------------HELPERS---------------------------------------------------------------------------------------------
                        
                        //  ListenForJoystickRightInput
                        //  ListenForJoystickLeftInput
                        //  ListenForDash
                        //  ListenForMove

    void ListenForJoystickRightInput()                          // joystickRightDirection, isPointer, isHeld, shouldDash
    {
        if (joystickRight.isPointerDown)
        {
            isJoystickRightHeld = true;
            joystickRightDirection = joystickRight.Direction;
        }
        if (joystickRight.isPointerUp)
        {
            isJoystickRightHeld = false;
            joystickRight.isPointerUp = false;
            shouldDash = true;
        }
    }

    void ListenForJoystickLeftInput()                           // joystickLeftDirection, isPointer, shouldMove
    {
        if (joystickLeft.isPointerDown)
        {
            joystickLeftDirection = joystickLeft.Direction;
            shouldMove = true;
        }
        if (joystickLeft.isPointerUp)
        {
            shouldMove = false;
            joystickLeft.isPointerUp = false;
        }
    }

    void ListenForDash()                                        // shouldDash, Dash
    {
        if (shouldDash)
        {
            Dash();
            shouldDash = false;
        }
    }

    void ListenForMove()                                        // shouldMove, Move
    {
        if (shouldMove)
        {
            Move();
        }
    }
}
