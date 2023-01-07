using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerReference : ScriptableObject {

    public GameObject player;

    public GameObject getPlayerPrefab() {
        return player;
    }
}
