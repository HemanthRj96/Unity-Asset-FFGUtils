using System.Collections.Generic;
using UnityEngine;

namespace FFG.Systems.Internal
{
    public class SaveData
    {
        public List<Vector3> Position = new List<Vector3>();
        public List<Quaternion> Rotation = new List<Quaternion>();
        public List<Vector3> Scale = new List<Vector3>();
        public List<float> Time = new List<float>();
        public Vector2 TimeStamp;

        public void CreateData(AnalogTransform data)
        {
            int totalKeys = data.Position.GetKeyCount();

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

            TimeStamp = data.TimeStamp;

            for (int i = 0; i < totalKeys; ++i)
            {
                Position.Add(new Vector3(xPos[i].value, yPos[i].value, zPos[i].value));
                Rotation.Add(new Quaternion(xRot[i].value, yRot[i].value, zRot[i].value, wRot[i].value));
                Scale.Add(new Vector3(xSca[i].value, ySca[i].value, zSca[i].value));
                Time.Add(xPos[i].time);
            }
        }

        public AnalogTransform GetData()
        {
            if (Position.Count == 0)
                return null;

            AnalogVector position = new AnalogVector();
            AnalogQuaternion rotation = new AnalogQuaternion();
            AnalogVector scale = new AnalogVector();
            Vector2 timeStamp = TimeStamp;

            int count = Position.Count;

            for (int i = 0; i < count; ++i)
            {
                Vector3 pos = Position[i];
                Quaternion rot = Rotation[i];
                Vector3 sca = Scale[i];
                float time = Time[i];

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

            AnalogTransform data = new AnalogTransform()
            {
                Position = position,
                Rotation = rotation,
                Scale = scale,
                TimeStamp = timeStamp
            };
            return data;
        }
    }
}
