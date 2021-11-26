using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    //---------------------------------CAMERA----------------------------------

    [SerializeField] CameraShake cameraShake;

    //--------------------------------TIMERS------------------------------------

    [SerializeField] private ChargeTimer rightChargeTimer;
    [SerializeField] private ChargeTimer leftChargeTimer;

    //--------------------------------MOVEMENT----------------------------------

    bool isMoving = false;

    //------------------------------------------------------Dash----------------
    [SerializeField] float initialDashForce = 20;
    [SerializeField] float dashForce = 20;
    bool shouldDash = false;
    //------------------------------------------------------Move----------------
    [SerializeField] float initialMoveForce = 1;
    [SerializeField] float moveForce = 1;
    [SerializeField] float moveForceAccelerator = 0.01f;
    [SerializeField] bool shouldMove = false;

    //-------------------------------ATTACK--------------------------------------

    [SerializeField] float collisionForce = 10;

    //-------------------------------EVENTS--------------------------------------

    public UnityEvent<float> OnPlayerMoveEnergyChangedEv;
    [SerializeField] Image MoveEnergyImage;

    public UnityEvent<float> onPlayerDashEnergyChangedEv;
    [SerializeField] Image DashEnergyImage;

    //-------------------------------MOVE-ENERGY-----------------------------------

    [SerializeField] float moveEnergy = 1;
    [SerializeField] float moveEnergyDecrementValue = 0.01f;
    [SerializeField] float moveEnergyDefaultRefill = 0.1f;

    //-------------------------------DASH-ENERGY-----------------------------------
    [SerializeField] float dashEnergy = 1;
    [SerializeField] float dashEnergyDecrementValue = 0.2f;
    [SerializeField] float dashEnergyDefaultRefill = 0.05f;

    //-------------------------------STATES-----------------------------------------

    enum State{
        Idle,
        Charging,
        Activated,
    }

    [SerializeField] State state;

    //------------------------------SPRITE-RENDERER---------------------------------

    [SerializeField] SpriteRenderer spriteRenderer;

    //----------------------------------TEST------------------------------------------------

    [SerializeField] float rightTestTimer = 0;
    [SerializeField] float magnitude = 0;

    //===============================START===============================================================================================
    // Start
    // Update
    // FixedUpdate
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        magnitude = rigidbody.velocity.magnitude;
        rightTestTimer = rightChargeTimer.timeElapsed;
        ListenForJoystickLeftInput();
        ListenForJoystickRightInput();
        //------------------------------------------------------------------StateListener-----
        ListenForJoystickRightTimer();
        Debug.Log(rightChargeTimer);
        ListenForIsMoving();
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
        ModifyDashEnergy(-dashEnergyDecrementValue);
    }

    void Move()                                                 // rigidbody, AddForce
    {
        rigidbody.AddForce(joystickLeftDirection.normalized * moveForce, ForceMode2D.Force);
        moveForce = moveForce+moveForceAccelerator;
        ModifyMoveEnergy(-moveEnergyDecrementValue);
    }

    //---------------------------------ENERGY-------------------------------------------

                        //  Modify Move Energy
                        //  Modify Dash Energy

    void ModifyMoveEnergy(float value)
    {
        moveEnergy += value;
        OnPlayerMoveEnergyChangedEv.Invoke(moveEnergy);
    }

    void ModifyDashEnergy(float value)
    {
        dashEnergy += value;
        onPlayerDashEnergyChangedEv.Invoke(dashEnergy);
    }

    // --------------------------------LISTENERS---------------------------------------------------------------------------------------------

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
            rightChargeTimer.Charge();
        }
        if (joystickRight.isPointerUp)
        {
            isJoystickRightHeld = false;
            joystickRight.isPointerUp = false;
            shouldDash = true;
            rightChargeTimer.Reset();
        }
    }

    void ListenForJoystickLeftInput()                           // joystickLeftDirection, isPointer, shouldMove
    {                                                           // moveForce, initialMoveForce
        if (joystickLeft.isPointerDown)
        {
            joystickLeftDirection = joystickLeft.Direction;
            shouldMove = true;
        }
        if (joystickLeft.isPointerUp)
        {
            shouldMove = false;
            joystickLeft.isPointerUp = false;
            moveForce = initialMoveForce;                       
        }
    }

    void ListenForDash()                                        // shouldDash, Dash
    {
        if (shouldDash)
        {
            Dash();
            shouldDash = false;
            // test
            isMoving = true;
        }

    }

    void ListenForMove()                                        // shouldMove, Move, MoveEnergyImage, ModifyMoveEnergy, moveEnergyRefill
    {
        if (shouldMove && moveEnergy>0)
        {
            Move();
        }
        else if(MoveEnergyImage.fillAmount<1 && !shouldMove) ModifyMoveEnergy(moveEnergyDefaultRefill);
    }

    void ListenForJoystickRightTimer()
    {
        
        //Debug.Log(rigidbody.velocity.magnitude);
        //Debug.Log(rightChargeTimer.timeElapsed);
       
        // DOESNT WORK!
        if (rigidbody.velocity.magnitude < 0.02f)
        {
            spriteRenderer.color = Color.white;
            state = State.Idle;
        }

        else if (rightChargeTimer.timeElapsed > 1)
        {
            spriteRenderer.color = Color.red;
            state = State.Activated;
        }
        else if ((rightChargeTimer.timeElapsed > 0 && rightChargeTimer.timeElapsed < 1))
        {
            spriteRenderer.color = Color.yellow;
            state = State.Charging;
        }
        
    }

    void ListenForIsMoving()
    {
        if (rigidbody.velocity.magnitude > 0.01f) isMoving = true;
        else isMoving = false;
    }

    // ---------------------------------------COLLISIONS--------------------------------------------------------

                        // OnCollisionEnter2D

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.5f));
        rigidbody.AddForce((transform.position - collision.gameObject.transform.position).normalized * collisionForce, ForceMode2D.Impulse);
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - transform.position).normalized * collisionForce, ForceMode2D.Impulse);
    }


}


// isMoving is experimental, please remove it if it serves no purpose.