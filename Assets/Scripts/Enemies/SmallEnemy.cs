using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmallEnemy : Monster  {
    Util util = new Util();

    void Start() {
        health = 5;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if ( col.transform.tag == util.skillTag ) {
            StartCoroutine(showDamageNumbers());
            StartCoroutine(flash());
            health -= 1;
        }
    }
}
