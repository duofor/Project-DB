using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Draggable {
    Util util = new Util();

    public List<Skill> weaponSkills; 
    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override List<Skill> getWeaponSkills() {
        return weaponSkills;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if ( col.transform.tag != util.playerTag || Inventory.instance.isInInventory(this) )
            return;
            
        Inventory.instance.addItemToInventory(this);
    }
}