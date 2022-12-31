using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    Util util = new Util();

    public List<Skill> weaponSkills; 
    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public List<Skill> getWeaponSkills() {
        return weaponSkills;
    }
}