using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmallEnemy : Monster  {
    Util util = new Util();

    void Start() {
        health = 5;
        //start invoking projectiles.
        InvokeRepeating("fireAt", 0f, 1f);
    }
}
