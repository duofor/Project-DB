using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public GroundItem[] itemsInChest;

    SpriteRenderer spriteRenderer;
    private Material outlineMaterial;
    private Material currentMaterial;

    bool canOpen;
    bool isAlreadyOpened;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentMaterial = spriteRenderer.material;
        outlineMaterial = Resources.Load<Material>("Material/Outline_Material");

        isAlreadyOpened = false;
    }

    void Update() {
        if ( canOpen && Input.GetKey(KeyCode.E) && !isAlreadyOpened) {
            isAlreadyOpened = true;
            StartCoroutine(openChest());
            Debug.Log("chest now opens");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        Player p = other.GetComponent<Player>();
        if ( p == null || isAlreadyOpened) return;

        spriteRenderer.material = currentMaterial;
        canOpen = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Player p = other.GetComponent<Player>();
        if ( p == null || isAlreadyOpened) return;


        spriteRenderer.material = outlineMaterial;
        canOpen = true;
    }

    IEnumerator openChest() {
        if ( itemsInChest.Length == 0 ){
            Destroy(gameObject);
            yield break;
        }

        foreach( GroundItem gi in itemsInChest ) {
            float random_X = Random.Range(0.1f, 0.2f);
            float random_Y = Random.Range(0.1f, 0.2f);

            GroundItem drop = Instantiate(gi, transform.position, transform.rotation);
            
            //maybe this can be done with a generic animation??? where we set the sprite only.
            float timeElapsed = 0f; float duration = 0.8f;
            while ( timeElapsed < duration ) {
                if ( drop.gameObject == null ) break;

                drop.transform.position = Vector3.Lerp(
                    transform.position,
                    new Vector3(
                        transform.position.x + (transform.position.x * random_X),
                        transform.position.y + (transform.position.y * random_Y),
                        0
                    ),
                    timeElapsed / duration
                );

                timeElapsed += Time.fixedDeltaTime;
                yield return null;
            }
        }
        
        Destroy(gameObject);
    }
}
