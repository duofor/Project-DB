using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingAttackSkill : Skill {
    Util util = new Util();

    void Start() {
        numberOfTargets = 2;
        skillDamage = 1;
        skillManaCost = 1;
    }
    
    public override IEnumerator startAttackAnimation() {
        yield return null;
    }

}