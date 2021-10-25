using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class DronePlayer : MonoBehaviour
{
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;

    //Stats
    #region
    [SerializeField] float dashforce = 5;
    [SerializeField] float chargedDashForce = 10;
    [SerializeField] float tempDashForce = 5;
    [SerializeField] float velocityDrag = 0.98f;
    [SerializeField] float velocityDragStart;
    [SerializeField] float velocityDragMinus;
    [SerializeField] float collisionForce = 15;
    #endregion

    [Header("Timers")]

    [SerializeField] private ChargeTimer rightChargeTimer;

    [SerializeField] private ChargeTimer leftChargeTimer; 

    [SerializeField] FloatingJoystick joystick;
    [SerializeField] Vector2 joystickDirection;

    [SerializeField] FloatingJoystick joystick2;
    [SerializeField] Vector2 joystickDirection2;

    [SerializeField] bool isDashing = false;
    [SerializeField] float dashingTime = 0.2f;

    [SerializeField] GameObject explosionParticles;
    [SerializeField] GameObject specialAttackParticles;

    [SerializeField] CameraShake cameraShake;

    [SerializeField] GameObject skillshot;
    [SerializeField] GameObject skillshot2;

    [SerializeField] bool isMoving = false;
    [SerializeField] bool isHeld = false;

    [SerializeField] bool isActivated = false;
    [SerializeField] float magnitudeNormalize = 4;

    [SerializeField] int joystickMode;
    [SerializeField] float curveForce = .1f;

    [SerializeField] private float energy = 1.0f;
    bool startHealthbarRefill = false;
    bool shouldHealthbarRefill = false;
    [SerializeField] float refillChargeTimer = 2;
    [SerializeField] float energyDecrementStep = 0.001f;
    [SerializeField] float energyDashDecrement = 0.1f;

    [SerializeField] Button specialButton;

    public UnityEvent<float> OnPlayerHealthChangedEv;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        velocityDragStart = velocityDrag;

        specialButton.onClick.AddListener(AreaBurstAttack);
    }

    // Update is called once per frame
    void Update()
    {
        Joystick1Handler();
        Joystick2Handler();
        HandleMovingState();
        rigidbody.velocity = rigidbody.velocity * velocityDrag;
        EnergyRefill();
    }

    void HandleMovingState()
    {
        if (rigidbody.velocity.magnitude > magnitudeNormalize) 
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (isMoving == false && isHeld==false)
        {
            leftChargeTimer.Reset();
            rightChargeTimer.Reset();
            velocityDrag = velocityDragStart;
            tempDashForce = dashforce;
            spriteRenderer.color = Color.white;
            isActivated = false;
        }
    }

    void Joystick1Handler()
    {
        if (joystick.isPointerDown)
        {
            isHeld = true;
            joystickDirection = joystick.Direction;
            ActivateSkillshot();
            rightChargeTimer.Charge();
        }
       
        if (joystick.isPointerUp)
        {
            if (isActivated)
            {
                velocityDrag = velocityDragMinus;
            }
            isHeld = false;
            DisableSkillshot();
            if (rightChargeTimer.IsCharged)
            {
                tempDashForce = chargedDashForce;
            }
            else
            {
                rightChargeTimer.Reset();
                tempDashForce = dashforce;
            }
            DashAttack(joystickDirection);
            joystick.isPointerUp = false;
            joystick.isPointerDown = false;
        }
        if (rightChargeTimer.IsCharged)
        {
            isActivated = true;
            spriteRenderer.color = Color.red;
            
        }
    }

    void Joystick2Handler()
    {
        if (joystickMode == 1)
        {
            Joystick2DashMode();
        }
        if (joystickMode == 2)
        {
            Joystick2AreaBurstMode();
        }
        if (joystickMode == 3){
            Joystick2CurveMode();
        }
    }

    void EnergyStepDecrement(){
        ModifyEnergy(-energyDecrementStep);
    }

    void EnergyDashDecrement()
    {
        ModifyEnergy(-energyDashDecrement);
    }

    void EnergyRefill(){
        if(shouldHealthbarRefill){
            StartCoroutine(waitforseconds());
        }   
        IEnumerator waitforseconds(){
            shouldHealthbarRefill=false;
            yield return new WaitForSeconds(refillChargeTimer);
            startHealthbarRefill=true;
        } 
        if(startHealthbarRefill && energy < 0.9f){
            ModifyEnergy(Time.deltaTime);
        }
        if(energy > 0.9f){
            startHealthbarRefill=false;
        }
    }

    void ModifyEnergy(float value) {
        energy += value;
        energy = Mathf.Clamp01(energy);
        OnPlayerHealthChangedEv.Invoke(energy);
    }
    

    void Joystick2CurveMode(){
        if (joystick2.isPointerDown && !Mathf.Approximately(energy, 0.0f)) {
            StopAllCoroutines();
            startHealthbarRefill=false;
            joystick2.isPointerUp = false;
            joystickDirection2 = joystick2.Direction;
            rigidbody.AddForce(joystickDirection2.normalized * curveForce,ForceMode2D.Force);
            EnergyStepDecrement();
            
        }
        if( joystick2.isPointerUp){
            shouldHealthbarRefill = true;
        }
    }

    void Joystick2AreaBurstMode(){
        if (joystick2.isPointerUp)
            {
                joystick2.isPointerUp = false;
                Instantiate(specialAttackParticles, transform.position, Quaternion.identity);
                rigidbody.velocity = Vector2.zero;
            }
    }

    void Joystick2DashMode()
    {
        if (joystick2.isPointerDown)
        {
            isHeld = true;
            joystickDirection2 = joystick2.Direction;
            ActivateSkillshot2();
            leftChargeTimer.Charge();
        }


        if (leftChargeTimer.IsCharged)
        {
            isActivated = true;
            //activationTimer = activationTimer + Time.deltaTime;
            spriteRenderer.color = Color.red;
        }
        if (joystick2.isPointerUp)
        {
            if (isActivated)
            {
                velocityDrag = velocityDragMinus;
            }
            isHeld = false;
            DisableSkillshot2();
            if (rightChargeTimer.IsCharged)
            {

                tempDashForce = chargedDashForce;
            }
            else
            {
                rightChargeTimer.Reset();
                tempDashForce = dashforce;
            }
            DashAttack(joystickDirection2);
            joystick2.isPointerUp = false;
            joystick2.isPointerDown = false;
        }
    }

    void AreaBurstAttack(){
        
               
                Instantiate(specialAttackParticles, transform.position, Quaternion.identity);
                rigidbody.velocity = Vector2.zero;
            
    }

    void DashAttack(Vector2 joystickDirection)
    {
        EnergyDashDecrement();
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(joystickDirection * tempDashForce, ForceMode2D.Impulse);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.5f));    
        if (isDashing)
        {
            collision.otherRigidbody.AddForce((transform.position - collision.gameObject.transform.position).normalized * collisionForce, ForceMode2D.Impulse);
            //collision.rigidbody.AddForce((collision.gameObject.transform.position - transform.position).normalized * collisionForce, ForceMode2D.Impulse);
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.setHealth(enemy.getHealth() - 1);
            if (enemy.getHealth() == 0)
            {
                Vector2 spawnPoint = gameObject.transform.position;
                GameObject temp = Instantiate(explosionParticles, spawnPoint, Quaternion.identity);
                Destroy(collision.gameObject);
            }
        }
        else
        {
            collision.otherRigidbody.AddForce((transform.position - collision.gameObject.transform.position).normalized * collisionForce, ForceMode2D.Impulse);
        }
    }

    void ActivateSkillshot()
    {
        skillshot.SetActive(true);
        skillshot.transform.rotation = Quaternion.LookRotation(Vector3.forward, joystickDirection);
    }

    void ActivateSkillshot2()
    {
        skillshot2.SetActive(true);
        skillshot2.transform.rotation = Quaternion.LookRotation(Vector3.forward, joystickDirection2);
    }

    void DisableSkillshot()
    {
        skillshot.SetActive(false);
    }

    void DisableSkillshot2()
    {
        skillshot2.SetActive(false);
    }
}
