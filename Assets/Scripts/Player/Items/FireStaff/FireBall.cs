using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill {
    Util util = new Util();

    public float skillDuration = 2f;
    public float skillSpeed = 2;

    void Start() {
        numberOfTargets = 1;
        skillDamage = 1;
        skillManaCost = 2;
    }
    
    public override IEnumerator startAttackAnimation() {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 skillStartingPosition = GameObject.Find("WeaponPoint").transform.position;
        // Quaternion weaponRotation = PlayerWeapon.instance.getPlayerWeapon().transform.rotation;
        Quaternion weaponRotation = transform.rotation;

        GameObject skill = Instantiate( getSkillAnimationPrefab(), skillStartingPosition, weaponRotation );

        float timeElapsed = 0f;
        while ( timeElapsed < skillDuration ) {
            skill.transform.position = Vector3.MoveTowards( 
                skillStartingPosition,
                targetPosition,
                timeElapsed / skillDuration * skillSpeed
            );
            
            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }
        Destroy(skill.gameObject);
        yield return null;
    }

}