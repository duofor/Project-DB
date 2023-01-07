
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : Projectile {

    public ParticleSystem dust;

    void Awake() {
        damage = 2;
        if (dust) {
            dust.Play();
        }
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
}
