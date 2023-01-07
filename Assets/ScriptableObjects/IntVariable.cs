using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class IntVariable : ScriptableObject {
    public int variable;

    public int getValue() {
        return variable;
    }
    public void setValue(int val) {
        variable = val;
    }
    
    public void decreaseValue(int val) {
        variable -= val;
    }
    public void increaseValue(int val) {
        variable += val;
    }
}
