using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour {
    Util util = new Util();

    public bool isSkillSelected = false;

    private SpriteRenderer spriteRenderer;
    Vector3 startMousePos;

    [SerializeField] private GameObject skillAnimation_Prefab;

    //test data
    public int skillLevel = 1;
    public int skillDamage = 1;
    public int skillManaCost = 1;
    public int numberOfTargets = 1;

    void Awake() {
        isSkillSelected = false;
    }

    public abstract IEnumerator startAttackAnimation();

    public GameObject getSkillAnimationPrefab() {
        return skillAnimation_Prefab;
    }
}