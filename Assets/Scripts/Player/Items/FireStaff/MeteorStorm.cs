using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorStorm : Skill {
    Util util = new Util();

    public float skillDuration = 1f;
    public float skillSpeed = 2;

    void Start() {
        numberOfTargets = 2;
        skillDamage = 2;
        skillManaCost = 1;
    }
    
    public override IEnumerator startAttackAnimation() {
        Vector3 targetPosition = new Vector3(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
            0
        );

        Vector3 skillStartingPosition = targetPosition + new Vector3(-0.50f, 1f, 0);
        Skill skill = Instantiate( this, skillStartingPosition, transform.rotation );

        float timeElapsed = 0f;
        while ( timeElapsed < skillDuration ) {
            skill.transform.position = Vector3.MoveTowards( 
                skillStartingPosition,
                new Vector3( targetPosition.x, targetPosition.y, 0 ),
                timeElapsed / skillDuration * skillSpeed
            );

            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }

        Destroy(skill.gameObject);
        yield return null;
    }

}