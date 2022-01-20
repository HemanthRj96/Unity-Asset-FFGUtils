using FFG.Systems.Internal;
using System.IO;
using UnityEngine;


namespace FFG.Systems
{
    /// <summary>
    /// Class which contains the implementation for saving transform data
    /// </summary>
    public class AnalogTransformDataSaver
    {
        /// <summary>
        /// Call this method to save transform data to a file in the form of json
        /// </summary>
        /// <param name="data">Target transform data</param>
        /// <param name="savePath">Target save path</param>
        public static void SaveData(AnalogTransform data, string savePath)
        {
            string jsonData = null;
            SaveData saveData = new SaveData();

            saveData.CreateData(data);
            jsonData = JsonUtility.ToJson(saveData);
            
            File.WriteAllText(savePath, jsonData);
        }

        /// <summary>
        /// Call this method to retrieve trasform data from a json file
        /// </summary>
        /// <param name="savePath">Target save path where the file is stored</param>
        public static AnalogTransform GetData(string savePath)
        {
            if (!File.Exists(savePath))
                return null;

            string jsonData = File.ReadAllText(savePath);
            SaveData SaveData = JsonUtility.FromJson<SaveData>(jsonData);

            return SaveData.GetData();
        }
    }
}