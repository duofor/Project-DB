using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestSpawner : MonoBehaviour {

    public Monster monsterPrefab;

    private Animator anim;
    [SerializeField] private string currentState;

    Vector3 offset;

    const string NEST_SPAWN = "Nest_Spawn";
    const string NEST_IDLE = "Nothing";

    void Start() {
        InvokeRepeating("SpawnMonster", 2f, 5f);
        anim = GetComponent<Animator>();
        offset = GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    void ChangeAnimationState(string newState) {
        // if ( currentState == newState ) return;

        // Debug.Log("Changing animation to " + newState );
        anim.Play(newState);
        // currentState = newState;
    }
    
    public void SpawnMonster() {
        Debug.Log("spawning");
        StartCoroutine(spawn());
    }

    IEnumerator spawn() {
        ChangeAnimationState(NEST_SPAWN);
        
        //Wait for them to enter the Attacking state
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(NEST_SPAWN)) {
            yield return null;
        }

        //Now wait for them to finish
        while (anim.GetCurrentAnimatorStateInfo(0).IsName(NEST_SPAWN)) {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0)) {
                break;
            }
            yield return null;
        }

        Instantiate( monsterPrefab, transform.position + offset, transform.rotation);
        ChangeAnimationState(NEST_IDLE);
    }
}
