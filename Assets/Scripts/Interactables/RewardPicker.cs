using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPicker : MonoBehaviour {

    public static RewardPicker instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public ItemDatabaseObject database;    

    public ItemObject pickRandomObject() {
        int random = Random.Range(0, database.ItemObjects.Length);

        ItemObject obj = database.ItemObjects[random];

        return obj;
    }

    public GameObject getRandomRewardObj() {
        //obj
        GameObject rewardObj = new GameObject();

        rewardObj.layer = 6; // Item layer
        rewardObj.transform.position = new Vector3(5555,5555,0);

        GroundItem item = rewardObj.AddComponent<GroundItem>();
        item.item = pickRandomObject();

        BoxCollider2D box = rewardObj.AddComponent<BoxCollider2D>();
        box.size = new Vector2(0.16f, 0.16f);
        box.isTrigger = true;

        Sprite uiDisplay = item.item.uiDisplay;

        //image
        GameObject obj_sprite = new GameObject();
        obj_sprite.transform.SetParent(rewardObj.transform);
        SpriteRenderer spriteRenderer = obj_sprite.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = uiDisplay;
        spriteRenderer.sortingOrder = 1;

        obj_sprite.transform.localPosition = new Vector3(0,0,0);
        obj_sprite.transform.localScale = new Vector3(0.16f, 0.16f, 0);

        return rewardObj;
    }

}
