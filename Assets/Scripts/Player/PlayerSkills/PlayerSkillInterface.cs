using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillInterface : MonoBehaviour {

    //will keep this public for testing purposes
    public List<PlayerSkillSlot> playerSkillSlots;

    void Update () {
        if ( Input.GetMouseButtonDown(0) ) {
            Skill skill = playerSkillSlots[0].getSkill();
            if ( skill != null  ) {
                StartCoroutine(skill.startAttackAnimation());
            }
        } else if ( Input.GetMouseButtonDown(1) ) {
            Skill skill = playerSkillSlots[1].getSkill();
            if ( skill != null  ) {
                StartCoroutine(skill.startAttackAnimation());
           }
        }   
    }

    public void addSkills( List<Skill> skills ) {
        clearPlayerSkills();
        int index = 0;

        foreach ( Skill skill in skills ) {
            if ( skill == null ) {
                continue;
            }

            Sprite skillSprite = skill.GetComponent<SpriteRenderer>().sprite;


            playerSkillSlots[index].setSkillSprite(skillSprite);
            // playerSkillSlots[index].setOrderInLayer(3);
            playerSkillSlots[index].setSkill(skill);
            index += 1;
        }
    }
    public void clearPlayerSkills() {
        if ( playerSkillSlots.Count == 0 )
            return;

        foreach ( PlayerSkillSlot pss in playerSkillSlots ) {
            pss.clearSkill();
            pss.clearSkillSprite();
        }
    }
}