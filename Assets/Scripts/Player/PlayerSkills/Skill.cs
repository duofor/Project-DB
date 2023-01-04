using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    Vector3 startMousePos;

    public Sprite skillIcon;

    //test data
    public int skillLevel = 1;
    public int skillDamage = 1;
    public int numberOfTargets = 1;

    public abstract IEnumerator startAttackAnimation();

    void OnTriggerEnter2D(Collider2D other) {
        Player p = other.GetComponent<Player>(); //ignore player
        Projectile pp = other.GetComponent<Projectile>(); //ignore enemy proj
        Skill ppp = other.GetComponent<Skill>(); //ignore self
        if ( p || pp || ppp) return;

        Monster m = other.GetComponent<Monster>();
        if (m) {
            m.takeDamage(1);
        }

        Destroy(gameObject);
    }
}   