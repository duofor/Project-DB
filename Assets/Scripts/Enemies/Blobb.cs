using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blobb : Monster  {
    Util util = new Util();

    public GameObject[] shootingPoints;
    public Vector2 pushForce = new Vector2(250f, 250f);

    void Start() {
        health = 5;
        //start invoking projectiles.
        float fireTime = Random.Range(1.5f, 3.5f);
        InvokeRepeating("fireAt", 0f, fireTime);
    }

    public override void fireAt() {
        if (readyForDestroy) return;

        foreach ( GameObject obj in shootingPoints ) {
            Projectile p = Instantiate(projectile, obj.transform.position, obj.transform.rotation);
            Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
            
            Vector2 direction = obj.transform.position - transform.position; 

            rb.AddForce(direction * pushForce);
        }
    }
}
