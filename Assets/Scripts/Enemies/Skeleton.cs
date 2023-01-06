using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skeleton : Monster  {
    Util util = new Util();

    void Start() {
        health = 5;
        //start invoking projectiles.
        float fireTime = Random.Range(1.5f, 3.5f);
        InvokeRepeating("fireAt", 0f, fireTime);
    }
}
