using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillInterface : MonoBehaviour {
    
    public PlayerSkillSlot[] skills;

    public void createSlots() {
        for (int i = 0; i < skills.Length; i++) {
            PlayerSkillSlot slot = skills[i];
            slot.setSkillSprite( slot.GetComponent<SpriteRenderer>().sprite );

            // AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            // AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            // AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            // AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            // AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
        }
    }
}
