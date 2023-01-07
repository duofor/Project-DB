using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    Util util = new Util();

    public static GameManager instance;

    public Chest rewardChest;
    public LevelLoader levelLoader;
    public Player player;

    bool canProceed = false; 

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }

        player = GameObject.Find("Player").GetComponent<Player>();
        levelLoader.setPlayer(player);

        Animator transition = GameObject.Find("BattleCanvas").GetComponent<Animator>();
        levelLoader.transition = transition;
    }

    void Start() {
        canProceed = false;
        StartCoroutine(levelLoader.waitAndLoadPlayerData());   // maybe in the future we can check if all the init is done, and then call this. 
        InvokeRepeating("checkAliveMonsters", 0f, 1f);
    }

    void checkAliveMonsters() {
        if (canProceed) return;

        GameObject[] mon = GameObject.FindGameObjectsWithTag(util.monsterTag);

        if (mon.Length > 0) {
            //.. game still plays
        } else {
            canProceed = true;
            levelLoader.gameObject.SetActive(true);
            rewardChest.enableAtPosition(new Vector3(0,0,0));
        }       
    }
}
