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
}
