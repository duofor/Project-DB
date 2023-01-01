using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheSlash : Skill {
    Util util = new Util();

    public float skillDuration = 15f;
    public float skillSpeed = 2;

    const string SCYTHE_SLASH = "Scythe_Slash";

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


        GameObject skill = Instantiate( 
            getSkillAnimationPrefab(),
            new Vector3(skillStartingPosition.x, skillStartingPosition.y, 0), 
            transform.rotation,
            helper.transform
        );

        skill.transform.localScale = new Vector3(
            skill.transform.localScale.x * 1.5f,
            skill.transform.localScale.y * 1.5f,
            1
        ); // double size 
        
        Animator animator = skill.GetComponent<Animator>();
        animator.speed = 3.5f;
        animator.Play(SCYTHE_SLASH);

        //Wait for them to enter the Attacking state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(SCYTHE_SLASH)) {
            yield return null;
        }

        //Now wait for them to finish
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(SCYTHE_SLASH)) {
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