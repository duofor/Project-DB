using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheSlash : Skill {
    Util util = new Util();

    public float skillDuration = 2f;
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
        // helper.transform.localScale = new Vector3(-1f,1f,1f); // flip cuz animation is reverse
        helper.transform.SetParent(playerTrans, true); // we want it to stick.

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - skillStartingPosition; 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        Quaternion rota = Quaternion.AngleAxis(angle, Vector3.forward); 

        Skill skill = Instantiate( 
            this,
            new Vector3(skillStartingPosition.x, skillStartingPosition.y, 0), 
            rota,
            helper.transform
        );

        float timeElapsed = 0f;
        Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();
        while( timeElapsed < skillDuration ) {
            if ( !skill ) {
                Destroy(helper.gameObject);
                yield break;
            }
            skill.transform.position = Vector3.MoveTowards(
                skill.transform.position,
                mousePos,
                Time.fixedDeltaTime * skillSpeed
            );

            timeElapsed += Time.fixedDeltaTime;

            yield return null;
        }

        Destroy(skill.gameObject);
        Destroy(helper.gameObject);
        yield return null;
    }

}