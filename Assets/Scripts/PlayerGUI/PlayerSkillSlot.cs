using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillSlot : MonoBehaviour {
    Util util = new Util();

    private Image image;
    [SerializeField] private Skill skill; //for debugga 
    private Material outlineMaterial;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSlotSprite;
    
    private Material initialMaterial;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
        
        defaultSlotSprite = image.sprite;

        outlineMaterial = Resources.Load<Material>("Material/Outline_Material");
        initialMaterial = spriteRenderer.material;
    }

    void OnMouseOver() {
        if ( transform.gameObject != null ) {
            image.material = outlineMaterial;
        }
    }

    void OnMouseExit() {
        if ( initialMaterial != null ) {
            image.material = initialMaterial;
        }
    }


    public void setSkillSprite ( Sprite sprite ) {
        image.sprite = sprite;
    }
    public void clearSkillSprite () {
        image.sprite = defaultSlotSprite;
    }   

    public void setSkill(Skill skillToSet) {
        skill = skillToSet;
        skill.isSkillSelected = false;
    }
    public Skill getSkill() {
        return skill;
    }

    public void clearSkill() {
        skill = null;
    }
}