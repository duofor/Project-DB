using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestSpawner : MonoBehaviour {

    public Monster monsterPrefab;

    public float spawnInterval = 5f;
    public int numberOfAllowedSpawns = 2;
    int spawned = 0;

    private Animator anim;
    Vector3 offset;

    const string NEST_SPAWN = "Nest_Spawn";
    const string NEST_IDLE = "Nothing";

    void Start() {
        InvokeRepeating("SpawnMonster", 2f, spawnInterval);
        anim = GetComponent<Animator>();
        offset = GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    void ChangeAnimationState(string newState) {
        anim.Play(newState);
    }
    
    public void SpawnMonster() {
        if ( spawned <= numberOfAllowedSpawns) {
            Debug.Log("spawning");
            StartCoroutine(spawn());
            spawned += 1;
        } else {
            Destroy(gameObject);
        }
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
