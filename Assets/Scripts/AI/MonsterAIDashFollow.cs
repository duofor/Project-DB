using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAIDashFollow : MonoBehaviour {
    
    private GameObject player;

    Rigidbody2D rb;

    public float movementSpeed = 25f;
    bool shouldMove;
    Vector3 playerPosition;

    void Awake() {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        shouldMove = false;

        float time = Random.Range(1.5f, 3.5f);
        InvokeRepeating("UpdateInfo", 1f, time);
    }

    void UpdateInfo() {
        playerPosition = player.transform.position;
        shouldMove = true;
    }

    void FixedUpdate() {
        if ( shouldMove ) {
            shouldMove = false;

            Vector2 direction = playerPosition - transform.position;
            rb.AddForce(direction * movementSpeed);
        }
    }

}
