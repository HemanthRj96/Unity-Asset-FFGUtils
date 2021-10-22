using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace FickleFrames.Systems
{
    public class BaseNode : Node
    {
        public string GUID;
        public bool entryPoint = false;
    }
}