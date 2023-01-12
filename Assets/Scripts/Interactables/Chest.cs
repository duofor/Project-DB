using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable {
    
    private Player player;
    public List<GroundItem> itemsInChest;

    void Start() {
        GameObject reward = RewardPicker.instance.getRandomRewardObj();
        
        itemsInChest.Add(reward.GetComponent<GroundItem>()); 
    }

    void Update() {
        if ( canOpen && Input.GetKey(KeyCode.E) && !isAlreadyOpened) {
            isAlreadyOpened = true;
            StartCoroutine(openChest());
            Debug.Log("chest now opens");
        }
    }

    IEnumerator openChest() {
        if ( itemsInChest.Count == 0 ){
            Destroy(gameObject);
            yield break;
        }

        foreach( GroundItem drop in itemsInChest ) {
            drop.transform.position = transform.position;
            //maybe this can be done with a generic animation??? where we set the sprite only.
            Rigidbody2D rb = drop.gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.drag = 5f;

            Debug.Log(drop.transform.position);
            Debug.Log(GameManager.instance.player);

            Vector2 direction = drop.transform.position - GameManager.instance.player.transform.position;
            rb.AddForce( direction * 100f );

        }
        
        Destroy(gameObject);
        yield return null;
    }

    public void enableAtPosition(Vector3 position) {
        Instantiate(this, position, transform.rotation);
    }
}
