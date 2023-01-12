using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    //this class is pretty much a highlightable not an interactable class.

    SpriteRenderer spriteRenderer;
    private Material outlineMaterial;
    private Material currentMaterial;

    public bool canOpen;
    public bool isAlreadyOpened;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentMaterial = spriteRenderer.material;
        outlineMaterial = Resources.Load<Material>("Material/Outline_Material");

        isAlreadyOpened = false;
    }

    public virtual void OnTriggerExit2D(Collider2D other) {
        Player p = other.GetComponent<Player>();
        if ( p == null || isAlreadyOpened) return;

        spriteRenderer.material = currentMaterial;
        canOpen = false;
    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        Player p = other.GetComponent<Player>();
        if ( p == null || isAlreadyOpened) return;

        spriteRenderer.material = outlineMaterial;
        canOpen = true;
    }
}
