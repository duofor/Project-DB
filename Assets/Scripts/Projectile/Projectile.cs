
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour {
    Util util = new Util();

    public FloatReference playerHp;
    public Vector3 targetPos;

    public float damage;
    public float projectileSpeed = 50f;
    public bool forceWasAdded = false;
    
    public bool isControlledFromChild = false;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        if ( animator ) {
            animator.Play("Attack");
        }

        //dmg struct
        damage = 1;
    
        // to be moved in childs
        targetPos = GameManager.instance.player.transform.position; // trash code to be changed later
    }

    void FixedUpdate() {
        if (isControlledFromChild) return;

        if ( targetPos != Vector3.zero && forceWasAdded == false) {
            forceWasAdded = true;
            Vector2 direction = (targetPos - transform.position).normalized; 
            Vector2 force = direction * projectileSpeed;

            rb.AddForce( force );
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if ( col.GetComponent<Monster>() || col.GetComponent<Projectile>() ) return;
        
        Debug.Log(col.transform.name);
        Destroy(gameObject);

        Player hit = col.transform.GetComponent<Player>();
        if ( hit ) {
            float tempDamage = (damage > 0) ? damage : 1;
            playerHp.receiveDamage(tempDamage);

        }
    }
}
