using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator transition;
    public float transitionTime = 1f;



    void Update() {
        if ( Input.GetKeyDown(KeyCode.L) ) {
            loadNextLevel();
        }
    }

    public void loadNextLevel() {
        StartCoroutine(loadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator loadScene( int sceneIndex ) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }   
}
