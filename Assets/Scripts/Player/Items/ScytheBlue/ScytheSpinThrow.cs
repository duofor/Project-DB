using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheSpinThrow : Skill {
    Util util = new Util();

    public float skillDuration = 2f;
    public float skillSpeed = 2;

    const string SCYTHE_THROWSPIN = "Scythe_ThrowSpin";


    void Start() {
        numberOfTargets = 1;
        skillDamage = 1;
        skillManaCost = 2;
    }
    
    public override IEnumerator startAttackAnimation() {
        GameObject weaponPoint = GameObject.Find("WeaponPoint");
        Transform playerTrans = GameObject.Find("Player").transform;

        Vector3 skillStartingPosition = weaponPoint.transform.position;

        //spawning a helper obj to hold the skill animation
        GameObject helper = new GameObject();
        helper.transform.position = skillStartingPosition;
        helper.transform.rotation = weaponPoint.transform.rotation; 
        helper.transform.localScale = new Vector3(-1f,1f,1f); // flip cuz animation is reverse
        helper.transform.SetParent(playerTrans, true); // we want it to stick.

        Skill skill = Instantiate( 
            this,
            new Vector3(skillStartingPosition.x, skillStartingPosition.y, 0), 
            transform.rotation,
            helper.transform
        );
        
        Animator animator = skill.GetComponent<Animator>();
        animator.Play(SCYTHE_THROWSPIN);

        //Wait for them to enter the Attacking state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(SCYTHE_THROWSPIN)) {
            yield return null;
        }

        //Now wait for them to finish
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(SCYTHE_THROWSPIN)) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 ) {
                break;
            }
            yield return null;
        }

        Destroy(skill.gameObject);
        Destroy(helper.gameObject);
        yield return null;
    }

}