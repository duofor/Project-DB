
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour {
    Util util = new Util();


    private Vector3 targetPos;

    public int damage;
    float projectileSpeed = 1.5f;
    

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        animator.Play("Attack");

        //dmg struct
        damage = 0;
    
        // to be moved in childs
        targetPos = GameObject.Find("Player").transform.position; // trash code to be changed later
    }

    void FixedUpdate() {
        if ( targetPos != Vector3.zero ) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                Time.fixedDeltaTime * projectileSpeed
            );
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        Destroy(gameObject);
    }
}
