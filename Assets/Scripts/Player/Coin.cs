using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

[System.Serializable]
[CreateAssetMenu(fileName = "New Collectable", menuName = "Collectable")]
public class CollectableObject : ScriptableObject {

    public string savePath;
    public int value;

    public void setValue(int c) {
        value = c;
    }
    public int getValue() {
        return value;
    }
    public void addToValue( int amount) {
        value += amount;
    }
    public void removeFromValue( int amount) {
        value -= amount;
    }

    public void Save() {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, value);
        stream.Close();
    }
    public void Load() {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath))) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            int coins = (int)formatter.Deserialize(stream);

            value = coins;

            stream.Close();
        }
    }
    public void Clear() {
        value = 0;
    }

}
