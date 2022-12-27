using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : ItemSlot {
    Util util = new Util();

    public override void setItem(Draggable i) {
        base.setItem(i);
        i.transform.SetParent(null);
        PlayerWeapon.instance.setPlayerWeapon(i);
        i.transform.SetParent(transform);
    }

    public override void clearItem() {
        base.clearItem();
        PlayerWeapon.instance.removePlayerWeapon();
    }

    public void clearSkills() {
        // GameController.instance.playerSkillManager.clearPlayerSkills();
    }
}
