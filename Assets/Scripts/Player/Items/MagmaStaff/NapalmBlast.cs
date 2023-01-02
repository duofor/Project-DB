using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmBlast : Skill {
    Util util = new Util();

    void Start() {
        numberOfTargets = 1;
        skillDamage = 1;
        skillManaCost = 2;
    }
    
    public override IEnumerator startAttackAnimation() {
        Vector3 targetPosition = Input.mousePosition;
        Vector3 skillStartingPosition = GameObject.Find("WeaponPoint").transform.position;

        Skill skill = Instantiate( this, skillStartingPosition, transform.rotation );

        float timeElapsed = 0f;
        float lerpDuration = 1.5f;
        while ( timeElapsed < lerpDuration ) {
            skill.transform.position = Vector3.Lerp( skill.transform.position, targetPosition, timeElapsed / lerpDuration );
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
        // fireAtTarget( target );
        Destroy(skill.gameObject);
    }
}