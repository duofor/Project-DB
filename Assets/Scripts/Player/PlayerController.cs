using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    public ParticleSystem dust;

    private Camera mainCamera;
    Rigidbody2D rb;    
    BoxCollider2D boxCollider;

    //state
    private enum State {
        Normal,
        Rolling,
    }
    private State state;
    // movement stuff
    Vector2 movementInput;
    float collisionOffset = 0.05f;
    public float movementSpeed = 2f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public GameObject weaponPoint;


    //dash
    Vector2 rollDirection;
    [SerializeField] private LayerMask dashLayerMask;

    //roll
    public float rollSpeed;
    Vector2 lastMoveDirection;
    public bool canRoll = true;

    public FloatReference rollTimer;
    public FloatReference rollCooldown; 

    void Awake() {
        state = State.Normal;
        mainCamera = Camera.main;
    }


    // Start is called before the first frame update
    void OnMove( InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        switch (state) {
            case State.Normal:
                if ( Input.GetKeyDown(KeyCode.Space) && canRoll) {
                    canRoll = false;
                    rollTimer.setValue(0f);
                    
                    createDust();
                    rollDirection = lastMoveDirection;
                    state = State.Rolling;
                    rollSpeed = 6f;
                }
                break;

            case State.Rolling:
                float rollSpeedDropMultiplier = 3f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                if ( !canMoveTowards(rollDirection, rollSpeed) ) {
                    state = State.Normal;
                    rb.velocity = Vector3.zero;
                }

                float rollSpeedMinimum = (movementSpeed / 2) + movementSpeed;
                if ( rollSpeed < rollSpeedMinimum ) {
                    state = State.Normal;
                    rb.velocity = Vector3.zero;
                }
                break;
        }

        if ( canRoll == false ) {
            rollTimer.setValue( rollTimer.value + Time.deltaTime );
            if ( rollTimer.value > rollCooldown.value ) {
                Debug.Log("resetting roll timer");
                rollTimer.setValue(2f);
                canRoll = true;
            }
        }
    }

    private void FixedUpdate() {
        switch ( state ) {
            case State.Rolling:
                if ( canMoveTowards(rollDirection, rollSpeed) ) {
                    rb.velocity = rollDirection * rollSpeed;
                }
                break;

            case State.Normal:
                if ( movementInput != Vector2.zero ) {
                    lastMoveDirection = movementInput;
                    bool moved = tryMove(movementInput);
                    
                    if (!moved) {
                        moved = tryMove( new Vector2(movementInput.x, 0) );
                        if ( !moved ) {
                            moved = tryMove( new Vector2(0, movementInput.y) );
                        }
                    }
                }
                break;
        }
        
    }

    void LateUpdate() {
        mainCamera.transform.position = new Vector3 (transform.position.x, transform.position.y, mainCamera.transform.position.z);
    }

    private bool tryMove(Vector2 direction) {
        //check colision
        if ( canMoveTowards(direction, movementSpeed) ) {
            rb.MovePosition( rb.position + direction * movementSpeed * Time.fixedDeltaTime );
            return true;
        }
        return false;
    }

    private bool canMoveTowards( Vector2 direction, float speed ) {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            speed * Time.fixedDeltaTime + collisionOffset
        );

        return (count == 0) ? true : false; 
    }

    void createDust() {
        dust.Play();
    }
}
