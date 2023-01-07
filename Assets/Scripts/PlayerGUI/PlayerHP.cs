using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour {

    public FloatReference currentHealth;
    public FloatReference maxHealth;

    float numberOfHearts;

    public GameObject healthPosition;
    public Sprite heart;
    public Sprite halfHeart;
    public Sprite heartEmpty;

    public float offset = 16f;

    List<GameObject> heartsList = new List<GameObject>();

    void Start() {
        numberOfHearts = maxHealth.value;
        generateHearts();
    }


    void Update() {
        if ( heartsList.Count != (int) maxHealth.value ) {
            generateHearts();
            numberOfHearts = maxHealth.value;
        }
        
        fillHearts();
    }

    void generateHearts() {
        clearCurrentHeartList();

        float heartOffset = 0f;
        for (int i = 0; i < numberOfHearts; i++) {
            GameObject obj = new GameObject();
            heartsList.Add(obj);

            obj.transform.position = healthPosition.transform.position + new Vector3(heartOffset, 0,0); 
            obj.transform.SetParent(transform);

            SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = heart;
            spriteRenderer.sortingOrder = 5;

            heartOffset += offset;
        }
    }

    void fillHearts() {
        for (int i = 0; i < heartsList.Count; i++) {
            Sprite sprite = heart;   
            if ( i >= currentHealth.value ) {
                sprite = heartEmpty;
            }
            heartsList[i].GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    void clearCurrentHeartList() {
        foreach ( GameObject go in heartsList ) {
            Destroy(go.gameObject);
        }

        heartsList = new List<GameObject>();
    }
}
