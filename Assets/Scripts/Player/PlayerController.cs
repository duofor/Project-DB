using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    [SerializeField] private Camera mainCamera;
    Rigidbody2D rb;    
    BoxCollider2D boxCollider;

    // movement stuff
    Vector2 movementInput;
    float collisionOffset = 0.05f;
    public float movementSpeed = 2f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void OnMove( InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainCamera.orthographicSize = 2f;
    }

    private void FixedUpdate() {
        mainCamera.transform.position = new Vector3 (transform.position.x, transform.position.y, mainCamera.transform.position.z); // Camera follows the player with specified offset position
        
        if ( movementInput != Vector2.zero ) {
            bool moved = tryMove(movementInput);

            if (!moved) {
                moved = tryMove( new Vector2(movementInput.x, 0) );
            
                if ( !moved ) {
                    moved = tryMove( new Vector2(0, movementInput.y) );
                }
            }
        }
        
    }

    private bool tryMove(Vector2 direction) {
        //check colision
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            movementSpeed * Time.fixedDeltaTime + collisionOffset
        );
        if ( count == 0 ) {
            rb.MovePosition( rb.position + direction * movementSpeed * Time.fixedDeltaTime );
            return true;
        }
        return false;
    }
}
