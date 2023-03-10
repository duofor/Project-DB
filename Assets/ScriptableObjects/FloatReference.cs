using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class FloatReference : ScriptableObject {
    public bool useConstant = true;
    public float constantValue;
    public FloatVariable variable;

    public float value {
        get {
            return useConstant ? constantValue : variable.value;
        }
    }

    public void receiveDamage(float amount) {
        if ( useConstant ) {
            constantValue -= amount;
        } else {
            variable.value -= amount;
        }
    }

    public void setValue(float val) {
        if ( useConstant ) {
            constantValue = val;
        } else {
            variable.value = val;
        }
    }
}
