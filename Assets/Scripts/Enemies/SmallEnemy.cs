using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : MonoBehaviour, IMonster  {
    Util util = new Util();

    int health = 5;






    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("yeh " + col.transform.name);
        if ( col.transform.tag == util.skillTag ) {
            DamageManager.instance.showDamageOnScreen(col.transform);
        }
    }
}
