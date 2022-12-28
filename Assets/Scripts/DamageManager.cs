using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageManager : MonoBehaviour {
    
    public static DamageManager instance;

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }
}
