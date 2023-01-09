using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackCounter : MonoBehaviour {

    public CollectableObject variable;
    public TextMeshProUGUI text;
    
    void Update() { // this can prob change into an event ??
        text.text = variable.getValue().ToString();
    }
}
