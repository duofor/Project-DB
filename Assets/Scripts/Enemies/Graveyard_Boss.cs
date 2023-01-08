using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Graveyard_Boss : Monster  {
    Util util = new Util();

    public GameObject[] shootingPoints;
    public GameObject[] adSpawnPoints;
    public GameObject transformed_shooting_point;

    public BossHpFillSetter bossHealthBarSetter_ref;

    public Monster ad_prefab;
    public Projectile sword_slash;  
    public Projectile berserk_projectile;  

    public Vector2 dashForce = new Vector2(100f, 100f);
    public float walkForce = 10f;

    //attacks speed
    public float waveSpeed_360 = 100f;
    public float wave_360_cooldown = 0.3f;
    float wave_360_timer = 0f;
    public float sword_slash_speed = 350f;

    //berserk
    public bool isBerserk = false;
    bool doOnce = true;
    bool isAttacking = false;
    bool isTransforming = false;

    Animator animator;
    float maxHealth;

    Rigidbody2D rb;
    ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    
    Player player_ref;

    //flip sprite
    public float flipTime = 0.5f;
    float countTime = 0f;
    float localScale_X = 0f;
    float localScale_Y = 0f;

    string ANIM_IDLE = "Graveyard_boss_idle";
    string ANIM_NULL = "null";
    string ANIM_ATTACK = "Graveyard_boss_attack";
    string ANIM_WALK = "Graveyard_boss_walk";
    string ANIM_TRANSFORMED = "Graveyard_boss_TRANSFORMED";
    string ANIM_TRANSFORMED_ATTACK = "Graveyard_boss_TRANSFORMED_Attack";
    string currentlyPlaying;

    public List<GameObject> randomMovePoints; 

// boss state logics
    public enum BossState {
        DASHING,
        WANDERING,
        ATTACKING,
        IDLE,
        NULL
    }
    public BossState currentState;
    public float dashingStateDuration = 5f;
    public float wanderStateDuration = 5f;
    float stateTimer;

    //dash counters
    public float dashCooldown = 1f;
    float dashTimer = 2f;

    public float wanderCooldown = 0.5f;
    float wanderTimer = 0.5f;
    bool isWandering = false;

    bool canSwitchState = false;
    public float switchStateCooldown = 10f;

    public bool isSwordSlashAnimationTrigger = false;

    void Start() {
        localScale_X = transform.localScale.x;
        localScale_Y = transform.localScale.y;

        maxHealth = health;
        rb = GetComponent<Rigidbody2D>();
        
        animator = GetComponent<Animator>();
        animator.Play(ANIM_NULL);
        currentlyPlaying = ANIM_NULL;

        currentState = BossState.IDLE;

        dashTimer = dashCooldown * 10;
        wanderTimer = wanderCooldown * 10;
        //start invoking projectiles.

        InvokeRepeating("stateControl", 1f, switchStateCooldown);

        player_ref = GameManager.instance.player;
    }

    public void stateControl() {
        StartCoroutine(shouldSwitchState());
    }

    new void Update() {
        base.Update();

        bossHealthBarSetter_ref.health = health;
        bossHealthBarSetter_ref.maxHealth = maxHealth;
    }

    void FixedUpdate() {
        if ( health <= maxHealth/2 ) isBerserk = true;

        if ( isBerserk && doOnce ) {
            doOnce = false;
            walkForce = walkForce * 2f;

            StartCoroutine(doTransition());
        }

        if ( isBerserk == true && isTransforming == true) return;

        BossState nextState = (canSwitchState) 
            ? decideNextAction(currentState)
            : currentState;

        switch ( nextState ) {

            case BossState.DASHING:
                if ( dashTimer > dashCooldown ) {
                    dashTimer = 0f;
                    StartCoroutine(dashAround());
                }
                dashTimer += Time.deltaTime;
                break;

            case BossState.ATTACKING:
                StartCoroutine(berserk_attack());
                changeAnimationState(ANIM_ATTACK);

                break;

            case BossState.WANDERING:
                if ( wanderTimer > wanderCooldown ) {
                    wanderTimer = 0f;
                    StartCoroutine(wander());
                }
                wanderTimer += Time.deltaTime;
                break;

            case BossState.IDLE:
                rb.velocity = new Vector2(0, 0);

                if ( isBerserk ) {
                    StartCoroutine(attack_360_hard());
                } else {
                    StartCoroutine(attack_360());
                }

                changeAnimationState(ANIM_IDLE);
                break;
            
            case BossState.NULL:
                StartCoroutine(dashAround());
                break;
        }


        if ( isSwordSlashAnimationTrigger ) {
            isSwordSlashAnimationTrigger = false;
            StartCoroutine(sword_slash_attack());
            changeAnimationState(ANIM_WALK);
        }

        currentState = nextState;

        flipSprite(false);
    }


    void changeAnimationState(string state) {
        if (currentlyPlaying == state) return;
        animator.Play(state);
        currentlyPlaying = state;
    }

    public override void fireAt() {
        if (readyForDestroy) return;
    }

    public IEnumerator dashAround() {
        changeAnimationState(ANIM_WALK);
        
        Vector3 destination = pickNextMovementPoint();
        Vector2 direction = destination - transform.position ;
        
        rb.AddForce(direction * dashForce * Time.fixedDeltaTime);

        yield return null;
    }

    public IEnumerator wander() {
        rb.velocity = new Vector2(0, 0);
        changeAnimationState(ANIM_WALK);
        
        if ( isWandering ) yield break;
        isWandering = true;

        Vector3 destination = pickNextMovementPoint();
        Vector2 direction = destination - transform.position;

        float timeElapsed = 0f;
        float duration = 3f;
        while ( timeElapsed < duration ) {
            transform.position = Vector3.Lerp(
                transform.position,
                destination,
                timeElapsed * Time.fixedDeltaTime
            );

            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }

        isWandering = false;
    }

    public void flipSprite(bool forceFlip) {
        countTime += Time.deltaTime;

        if ( countTime > flipTime || forceFlip) { //atempt to make sprite not flip every milisec
            countTime = 0f;

            if (rb.velocity.x >= 0.01f ) {
                transform.localScale = new Vector3( - localScale_X, localScale_Y, 1f);
            } else if (rb.velocity.x <= -0.01f ) {
                transform.localScale = new Vector3( localScale_X, localScale_Y, 1f);
            }
        }
    }

    public BossState decideNextAction(BossState currentState) {
        BossState nextState = BossState.NULL;

        int random = Random.Range(0, 2);

        if ( currentState == BossState.IDLE ) {
            nextState = BossState.DASHING;

        } else if ( currentState == BossState.DASHING ) {
            nextState = BossState.WANDERING;

            if (random == 0 ) {
                nextState = BossState.ATTACKING;
            }
        
        } else if ( currentState == BossState.WANDERING ) {

            nextState = BossState.IDLE;
            if (random == 1 && !isBerserk) {
                nextState = BossState.ATTACKING;
            }

        } else if ( currentState == BossState.ATTACKING ) {
            nextState = BossState.DASHING;
        }

        canSwitchState = false;
        return nextState;
    }

    IEnumerator shouldSwitchState() {
        if (canSwitchState == true) yield break; 
        
        float timeElapsed = 0f;
        while ( timeElapsed > switchStateCooldown ) {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canSwitchState = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Graveyard_Boss>()) return;
    }

    private bool canMoveTowards( Vector2 direction, float speed ) {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            speed * Time.fixedDeltaTime + 0.05f
        );

        return (count == 0) ? true : false; 
    }

    IEnumerator attack_360() {
        wave_360_timer += Time.deltaTime;

        if ( wave_360_timer > wave_360_cooldown ) {
            wave_360_timer = 0f;
            foreach ( GameObject go in shootingPoints ) {
                Projectile proj = Instantiate( 
                    projectile,
                    go.transform.position,
                    go.transform.rotation
                );
                proj.isControlledFromChild = true; 

                Vector2 direction = (go.transform.position - transform.position).normalized;
                
                Rigidbody2D gorb = proj.GetComponent<Rigidbody2D>();
                gorb.AddForce(direction * waveSpeed_360 * Time.fixedDeltaTime);
            }
        }

        yield return null;
    }
    IEnumerator attack_360_hard() {
        wave_360_timer += Time.deltaTime;
        wave_360_timer += Time.deltaTime;

        if ( wave_360_timer > wave_360_cooldown ) {
            wave_360_timer = 0f;
            foreach ( GameObject go in shootingPoints ) {
                Projectile proj = Instantiate( 
                    projectile,
                    go.transform.position,
                    go.transform.rotation
                );

                proj.isControlledFromChild = true; 

                Vector2 direction = (go.transform.position - transform.position).normalized;
                direction += new Vector2(
                    Random.Range(0f, 0.35f),
                    Random.Range(0f, 0.35f)
                );

                Rigidbody2D gorb = proj.GetComponent<Rigidbody2D>();
                gorb.AddForce(direction * (waveSpeed_360 + waveSpeed_360 * 0.7f)  * Time.fixedDeltaTime);
            }
        }

        yield return StartCoroutine(dashAround());
    }

    IEnumerator sword_slash_attack() {
        if (isBerserk) yield break;
        
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.2f);

        Vector2 direction = player_ref.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        Quaternion rota = Quaternion.AngleAxis(angle + 130, Vector3.forward);
         
        Projectile slash = Instantiate(
            sword_slash,
            shootingPoint.transform.position,
            rota
        );
        slash.isControlledFromChild = false;

        rb.AddForce(-direction / 95); //so we turn towards the player
        flipSprite(true);

        yield return null;
    }

    IEnumerator berserk_attack() {
        if (!isBerserk || isAttacking ) yield break;
        
        isAttacking = true;
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.2f);

        float timeElapsed = 0f;
        float duration = 0.8f;
        while ( timeElapsed < duration ) {
            Vector2 direction = player_ref.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
            Quaternion rota = Quaternion.AngleAxis(angle + 130, Vector3.forward);
            
            Projectile slash = Instantiate(
                berserk_projectile,
                transformed_shooting_point.transform.position,
                rota
            );
            slash.isControlledFromChild = false;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
    }

    Vector3 pickNextMovementPoint() {
        int random = Random.Range(0, randomMovePoints.Count);

        Vector3 position = randomMovePoints[random].transform.position;

        return position;
    }

    void spawnSkeletons( ) {
        foreach ( GameObject go in adSpawnPoints ) {
            Monster mon = Instantiate(
                ad_prefab,
                go.transform.position,
                go.transform.rotation
            );
        }
        
    }

    IEnumerator doTransition() {
        isTransforming = true;
        Time.timeScale = 0;
        rb.velocity = new Vector2(0, 0);
        
        float initialSize = Camera.main.orthographicSize;
        Vector3 initialPos = Camera.main.transform.position;

        float timeElapsed = 0f;
        float duration = 1f;

        while ( timeElapsed < duration ) {
            
            Camera.main.orthographicSize -= 0.005f;
            timeElapsed += 0.01f;

            if ( Camera.main.orthographicSize <= 0.7f ) 
                yield break;

            yield return null;
        }

        Camera.main.transform.position = transform.position;
        
        projectile = berserk_projectile;
        ANIM_IDLE = ANIM_TRANSFORMED;
        ANIM_WALK = ANIM_TRANSFORMED;
        ANIM_ATTACK = ANIM_TRANSFORMED_ATTACK;
        
        changeAnimationState(ANIM_IDLE);
        Time.timeScale = 1;

        spawnSkeletons();
        Camera.main.transform.position = initialPos;
        Camera.main.orthographicSize = initialSize;

        isTransforming = false;

    }

    // IEnumerator slowlyFade( GameObject go ) {

    //     // while ( timeElapsed < duration ) {
    //     //     Sprite sprite = go.GetComponent<SpriteRenderer>().sprite;
        
        
    //     // }   
    // }
}
