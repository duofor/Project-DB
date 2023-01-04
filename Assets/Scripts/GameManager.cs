using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    Util util = new Util();

    public LevelLoader levelLoader; 

    void Start() {
        Player p = GameObject.Find("Player").GetComponent<Player>();
        Animator transition = GameObject.Find("BattleCanvas").GetComponent<Animator>();
        levelLoader.player = p;
        levelLoader.transition = transition;
        StartCoroutine(levelLoader.waitAndLoadPlayerData());   // maybe in the future we can check if all the init is done, and then call this. 
        InvokeRepeating("checkAliveMonsters", 0f, 1f);
    }

    void checkAliveMonsters() {
        GameObject[] mon = GameObject.FindGameObjectsWithTag(util.monsterTag);

        if (mon.Length > 0) {
            //.. game still plays
        } else {
            levelLoader.gameObject.SetActive(true);
        }       
    }
}
