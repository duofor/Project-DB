using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Player player;
    BoxCollider2D boxCollider2D;
    public Animator transition;
    public float transitionTime = 1f;

    void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start() {
        if ( SceneManager.GetActiveScene().buildIndex == 0 ){
            gameObject.SetActive(false);
            return; //dont load on first scene;
        }      
    }

    public void loadNextLevel() {
        StartCoroutine(loadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator loadScene( int sceneIndex ) {
        transition.SetTrigger("Start");
        player.savePlayerData();        
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("cur");
        Player p = other.GetComponent<Player>();
        if (p == null) return;

        loadNextLevel();
    }

    public IEnumerator waitAndLoadPlayerData() {
        if ( SceneManager.GetActiveScene().buildIndex == 0 ) 
            yield break;

        yield return new WaitForSeconds(0.1f);
        player.loadPlayerData();
        gameObject.SetActive(false);
    }


}
