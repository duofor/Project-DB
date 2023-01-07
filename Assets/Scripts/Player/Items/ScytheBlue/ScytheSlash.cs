using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheSlash : Skill {
    Util util = new Util();

    public float skillDuration = 2f;
    public float skillSpeed = 20f;

    const string SCYTHE_SLASH = "Scythe_Slash";

    void Start() {
        numberOfTargets = 1;
        skillDamage = 1;
    }
    
    public override IEnumerator startAttackAnimation() {
        GameObject weaponPoint = GameObject.Find("WeaponPoint");
        Transform playerTrans = GameManager.instance.player.transform;

        Vector3 skillStartingPosition = weaponPoint.transform.position;

        //spawning a helper obj to hold the skill animation
        GameObject helper = new GameObject();
        helper.transform.position = skillStartingPosition;
        helper.transform.rotation = weaponPoint.transform.rotation; 
        helper.transform.SetParent(playerTrans, true); // we want it to stick.

        //rotation
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
        
        // skill movement
        Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();

        if ( skill == null ) { //this can happen when ontrigger2d destroyes the obj
            Destroy(helper.gameObject); 
        }

        Vector2 force = direction.normalized * skillSpeed; // normalization makes the speed constant regardless of mouse pos
        rb.AddForce( force );

        Destroy(skill.gameObject, 3);
        Destroy(helper.gameObject, 3);

        yield return null;
    }

}