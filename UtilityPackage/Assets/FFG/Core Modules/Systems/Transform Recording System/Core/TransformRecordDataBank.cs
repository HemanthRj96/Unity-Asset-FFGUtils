using FFG.Systems.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace FFG.Systems
{
    public class TransformRecordDataBank : ScriptableObject
    {
        private Dictionary<string, TransformRecordData> _savedData = new Dictionary<string, TransformRecordData>();

        public void SaveData(string saveName, TransformRecordData saveData)
        {
            if (_savedData.ContainsKey(saveName))
            {
                Debug.LogWarning("Replacing old data!!");
                _savedData[saveName] = saveData;
            }
            else
                _savedData.Add(saveName, saveData);
        }

        public TransformRecordData GetData(string saveName)
        {
            if (_savedData.ContainsKey(saveName))
                return _savedData[saveName];

            Debug.LogError("The save data do not exist");
            return null;
        }
    }
}