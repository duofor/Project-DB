using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public static PlayerWeapon instance;

    [SerializeField] private Draggable playerWeapon;
    [SerializeField] private GameObject weaponPoint;

    public float radius;
 
    private Transform pivot;
    
    // for calculating weapon rotation
    Vector3 playerBox;

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }

        playerBox = GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    public void setPlayerWeapon(Draggable weapon) {
        if ( playerWeapon ) {
            Destroy(playerWeapon.gameObject);
        }

        playerWeapon = Instantiate( weapon );
        playerWeapon.transform.position = weaponPoint.transform.position;
        playerWeapon.transform.SetParent(transform);


        List<Skill> weaponSkills = playerWeapon.getWeaponSkills();
        PlayerSkillManager.instance.addSkills(weaponSkills);
    }

    public void removePlayerWeapon() {
        if ( playerWeapon ) {
            Destroy(playerWeapon.gameObject);
            playerWeapon = null;
            PlayerSkillManager.instance.clearPlayerSkills();
        }
    }

    public Draggable getPlayerWeapon() {
        return playerWeapon;
    }
}