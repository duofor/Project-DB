using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MonsterCharge : MonoBehaviour {
    Util util = new Util();

    private Transform target;

    public float chargeSpeed = 5000f;

    Rigidbody2D rb;
    Color initialColor;
    SpriteRenderer spriteRenderer;

    void Start() {
        target = GameObject.Find("Player").transform; 
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("charge", 0f, 5f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    void charge() {
        target = GameObject.Find("Player").transform; 
        StartCoroutine(chargeAt(target));
    }

    IEnumerator chargeAt(Transform trans) {

        Vector2 direction = ((Vector2) (target.position - transform.position)).normalized;
        Vector2 force = direction * chargeSpeed;

        float timeElapsed = 0f;
        float chargeDuration = 1.5f;

        spriteRenderer.color = Color.white;
        while ( timeElapsed < chargeDuration ) {
            // start charge animation here

            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }
        
        spriteRenderer.color = initialColor;
        // play a charge animation
        rb.AddForce(force);
    }

    void OnTriggerEnter2D(Collider2D col) {
        rb.velocity = Vector3.zero;
    }


}
