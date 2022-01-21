using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace FFG.Systems.Internal
{
    public class TransformDataSaver
    {
        /// <summary>
        /// Data container to store transform data
        /// </summary>
        [System.Serializable]
        public struct SaveData
        {
            public List<Vector3> Position;
            public List<Quaternion> Rotation;
            public List<Vector3> Scale;
            public List<float> Time;
            public Vector2 TimeStamp;
        }


        /// <summary>
        /// Method to convert AnalogTransform into JSON
        /// </summary>
        /// <param name="data">Target data</param>
        /// <param name="filepath">Target filepath</param>
        /// <param name="filename">Target filename</param>
        public static void WriteData(AnalogTransform data, string filepath, string filename)
        {
            int totalKeys = data.Position.GetKeyCount();

            List<Vector3> position = new List<Vector3>();
            List<Quaternion> rotation = new List<Quaternion>();
            List<Vector3> scale = new List<Vector3>();
            List<float> time = new List<float>();
            Vector2 timeStamp = data.TimeStamp;

            Keyframe[] xPos = data.Position.X.keys;
            Keyframe[] yPos = data.Position.Y.keys;
            Keyframe[] zPos = data.Position.Z.keys;

            Keyframe[] xRot = data.Rotation.X.keys;
            Keyframe[] yRot = data.Rotation.Y.keys;
            Keyframe[] zRot = data.Rotation.Z.keys;
            Keyframe[] wRot = data.Rotation.W.keys;

            Keyframe[] xSca = data.Scale.X.keys;
            Keyframe[] ySca = data.Scale.Y.keys;
            Keyframe[] zSca = data.Scale.Z.keys;


            for (int i = 0; i < totalKeys; ++i)
            {
                position.Add(new Vector3(xPos[i].value, yPos[i].value, zPos[i].value));
                rotation.Add(new Quaternion(xRot[i].value, yRot[i].value, zRot[i].value, wRot[i].value));
                scale.Add(new Vector3(xSca[i].value, ySca[i].value, zSca[i].value));
                time.Add(xPos[i].time);
            }

            SaveData saveData = new SaveData()
            {
                Position = position,
                Rotation = rotation,
                Scale = scale,
                Time = time,
                TimeStamp = timeStamp
            };

            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);

            JsonSaver.WriteData(saveData, filepath, filename);
            Debug.Log($"Save complete! Recordings have been saved to : {filepath}/{filename}.json");
        }


        /// <summary>
        /// Returns AnalogTransform from JSON
        /// </summary>
        /// <param name="filepath">Target filepath</param>
        /// <param name="filename">Target filename</param>
        public static AnalogTransform ReadData(string filepath, string filename)
        {
            AnalogVector position = new AnalogVector();
            AnalogQuaternion rotation = new AnalogQuaternion();
            AnalogVector scale = new AnalogVector();

            // Read data
            JsonSaver.ReadData(filepath, filename, out SaveData data);

            if (data.Position == null || data.Position.Count == 0)
            {
                Debug.LogError("Data loading failed!!");
                return null;
            }

            int count = data.Position.Count;

            for (int i = 0; i < count; ++i)
            {
                Vector3 pos = data.Position[i];
                Quaternion rot = data.Rotation[i];
                Vector3 sca = data.Scale[i];
                float time = data.Time[i];

                position.X.AddKey(time, pos.x);
                position.Y.AddKey(time, pos.y);
                position.Z.AddKey(time, pos.z);

                rotation.X.AddKey(time, rot.x);
                rotation.Y.AddKey(time, rot.y);
                rotation.Z.AddKey(time, rot.z);
                rotation.W.AddKey(time, rot.w);

                scale.X.AddKey(time, sca.x);
                scale.Y.AddKey(time, sca.y);
                scale.Z.AddKey(time, sca.z);
            }

            return new AnalogTransform()
            {
                Position = position,
                Rotation = rotation,
                Scale = scale,
                TimeStamp = data.TimeStamp
            };
        }
    }
}
