using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skeleton : Monster  {
    Util util = new Util();


    void Start() {
        fireCooldown = Random.Range(0.5f, 1.5f);
        health = 5;
        
        //start invoking projectiles.
        InvokeRepeating("fireAt", 0f, fireCooldown);
    }
}
